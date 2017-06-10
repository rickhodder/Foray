using System.Linq;

namespace Foray.Common
{
    public class RelationshipState : SchemaParserState
    {
        public RelationshipState(IContext<IStringReader, Schema> context) : base(context)
        {
        }

        public override void Handle()
        {
            Context.Variables.Remove("lastentity");
            var test = Context.Input.CurrentLine.Trim();

            var oneToOne = test.Contains('-') && !test.Contains('>') && !test.Contains('<');
            var oneToMany = test.Contains("->") || (test.Contains(">") && !test.Contains('<'));
            var manyToMany = test.Contains("<") && test.Contains(">");

            string[] pieces;
            var entityName1 = "";
            var entityName2 = "";
            var entityName3 = "";

            var relType = RelationshipType.Self;

            if (oneToOne)
            {
                relType = RelationshipType.OneToOne;
                pieces = test.Split('-');
                entityName1 = pieces[0];
                entityName2 = pieces[1];
            }

            if (oneToMany)
            {
                relType = RelationshipType.OneToMany;
                var separator = (test.Contains('-') ? "->" : ">");
                var divider = test.IndexOf(separator);
                entityName1 = test.Substring(0, divider);
                entityName2 = test.Substring(divider + separator.Length);
            }


            if (manyToMany)
            {
                relType = RelationshipType.ManyToMany;
                pieces = Context.Input.CurrentLine.Split(new[] { '<', '>' });

                if (pieces.Length == 2)
                {
                    entityName1 = pieces[0];
                    entityName2 = pieces[1];
                }
                if (pieces.Length == 3)
                {
                    entityName1 = pieces[0];
                    entityName2 = pieces[1];
                    entityName3 = pieces[2];
                }
            }

            var relationship = Context.Output.AddRelationship(relType, entityName1, entityName2, entityName3);

            Context.SetState(new FindingState(Context));
        }
    }
}