using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foray.Common
{
    public interface IContext<TInput, TOutput>
    {
        Dictionary<string, object> Variables { get; set; }
        List<string> ErrorMessages { get; set; }
        bool Finished { get; set; }
        TInput Input { get; set; }
        TOutput Output { get; set; }
        IState<TInput, TOutput> CurrentState { get; set; }
        void SetState(IState<TInput, TOutput> state);
        TOutput Execute();
    }

    public abstract class AbstractContext<TInput, TOutput> : IContext<TInput, TOutput>
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, Object>();
        public bool Finished { get; set; }
        public TInput Input { get; set; }
        public TOutput Output { get; set; }
        public IState<TInput, TOutput> CurrentState { get; set; }

        public void SetState(IState<TInput, TOutput> state)
        {
            CurrentState = state;
        }

        public TOutput Execute()
        {
            while (!Finished)
            {
                CurrentState.Handle(this);
            }

            return Output;
        }
    }

    public class SchemaContext : AbstractContext<StringReader, Schema>
    {

    }

    public interface IState<TInput, TOutput>
    {
        void Handle(IContext<TInput, TOutput> context);
    }

    public abstract class SchemaParserState : IState<StringReader, Schema>
    {
        public abstract void Handle(IContext<StringReader, Schema> context);
    }

    public class SchemaStartState : SchemaParserState
    {
        private string _dsl;

        public SchemaStartState(string dsl)
        {
            _dsl = dsl;
        }

        public override void Handle(IContext<StringReader, Schema> context)
        {
            context.Input = new StringReader(_dsl);
            context.Output = new Schema();
            context.SetState(new FindingState());
        }
    }

    public class FindingState : SchemaParserState
    {
        public override void Handle(IContext<StringReader, Schema> context)
        {
            while (!context.Input.IsDone())
            {
                if (!string.IsNullOrEmpty(context.Input.GetLine()))
                {
                    context.SetState(new DeterminingState());
                    return;
                };
            }

            context.Finished = true;
        }
    }

    public class DeterminingState : SchemaParserState
    {
        public override void Handle(IContext<StringReader, Schema> context)
        {
            if (context.Input.CurrentLine.StartsWith("\t"))
            {
                context.SetState(new FieldState());
                return;
            }
            if (context.Input.CurrentLine.Contains("<") || context.Input.CurrentLine.Contains(">"))
            {
                context.SetState(new RelationshipState());
                return;
            }

            context.SetState(new EntityState());
        }
    }

    public class EntityState : SchemaParserState
    {
        public override void Handle(IContext<StringReader, Schema> context)
        {
            var entityName = context.Input.CurrentLine.Trim();
            var entity = new Entity { Name = entityName };
            context.Output.Entities.Add(entity);
            context.Variables["lastentity"] = entity;
            context.SetState(new FindingState());
        }
    }

    public class FieldState : SchemaParserState
    {
        public override void Handle(IContext<StringReader, Schema> context)
        {
            if (context.Variables.ContainsKey("lastentity"))
            {
                var table = (Entity)context.Variables["lastentity"];
                var fieldName = context.Input.CurrentLine.Trim();
                var field = new EntityField { Name = fieldName };
                table.Fields.Add(field);
                context.SetState(new FindingState());
            }
            else
            {
                context.SetState(new ErrorState($"Field found {context.Input.CurrentLine.Trim()} but no table"));
            }
        }
    }

    public class RelationshipTypeMapper : Dictionary<string, RelationshipType>
    {
        public RelationshipTypeMapper()
        {
            this["-"] = RelationshipType.OneToOne;
            this[">"] = RelationshipType.OneToMany;

        }
    }

    public class RelationshipState : SchemaParserState
    {

        public override void Handle(IContext<StringReader, Schema> context)
        {
            context.Variables.Remove("lastentity");
            var test = context.Input.CurrentLine.Trim();

            bool oneToOne = test.Contains('-') && !test.Contains('>') && !test.Contains('<');
            bool oneToMany = test.Contains("->") || (test.Contains(">") && !test.Contains('<'));
            bool manyToMany = test.Contains("<") && test.Contains(">");

            string[] pieces;
            string entityName1 = "";
            string entityName2 = "";
            string entityName3 = "";

            RelationshipType relType = RelationshipType.Self;

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
                pieces = context.Input.CurrentLine.Split(new[] { '<', '>' });

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

            var relationship = context.Output.AddRelationship(relType, entityName1, entityName2, entityName3);

            context.SetState(new FindingState());
        }
    }


    public class ErrorState : SchemaParserState
    {
        private string _message;

        public ErrorState(string message)
        {
            _message = message;
        }
        public override void Handle(IContext<StringReader, Schema> context)
        {
            context.ErrorMessages.Add(_message);
            context.Finished = true;

        }
    }
    
    public class StringReader : IDisposable
    {
        StreamReader _reader;

        public string CurrentLine { get; private set; }

        public StringReader(string content)
        {
            var ms = new MemoryStream();

            ms.Write(Encoding.Default.GetBytes(content.ToCharArray(), 0, content.Length), 0, content.Length);
            ms.Position = 0;
            _reader = new StreamReader(ms);
        }

        public string GetLine()
        {
            CurrentLine = "";

            while (!_reader.EndOfStream)
            {

                char peek = (char)_reader.Peek();
                if (peek == ' ')
                {
                    _reader.Read();
                    continue;
                }

                if (peek == '\r' || peek == '\n')
                {
                    // skip over it
                    _reader.Read();

                    peek = (char)_reader.Peek();

                    if (peek == '\n')
                    {
                        // skip over it
                        _reader.Read();
                    }
                    break;
                }
                else
                {
                    CurrentLine += (char)_reader.Read();
                }
            }


            return CurrentLine;
        }

        public bool IsDone()
        {
            return _reader.EndOfStream;
        }

        public void Dispose()
        {
            _reader?.Close();
            _reader = null;
        }
    }

    public class Schema
    {
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public List<Relationship> Relationships { get; set; } = new List<Relationship>();

        public Entity AddEntity(string name)
        {
            var entity = FindEntity(name);
            if (entity != null)
            {
                return entity;
            }

            entity = new Entity { Name = name.ToLower().Trim() };
            Entities.Add(entity);
            return entity;
        }

        public Entity FindEntity(string name)
        {
            return Entities.FirstOrDefault(e => e.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public Relationship AddRelationship(RelationshipType reltype, string first, string second, string nested = "")
        {
            var entityFirst = AddEntity(first);
            var entitySecond = AddEntity(second);
            Entity entityNested = string.IsNullOrEmpty(nested) ? null : AddEntity(nested);

            var r = new Relationship
            {
                FirstEntity = entityFirst,
                SecondEntity = entitySecond,
                NestedEntity = entityNested,
                RelationshipType = reltype
            };

            Relationships.Add(r);

            return r;
        }
    }

    public class Entity
    {
        public string Name { get; set; }
        public List<EntityField> Fields { get; set; } = new List<EntityField>();
    }

    public class EntityField
    {
        public string Name { get; set; }
    }

    public class Relationship
    {
        public RelationshipType RelationshipType { get; set; }
        public Entity FirstEntity { get; set; }
        public Entity SecondEntity { get; set; }
        public Entity NestedEntity { get; set; }
    }

    public enum RelationshipType
    {
        OneToOne,
        OneToMany,
        ManyToMany,
        Self
    }
}
