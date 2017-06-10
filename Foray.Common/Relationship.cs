namespace Foray.Common
{
    public class Relationship
    {
        public RelationshipType RelationshipType { get; set; }
        public Entity FirstEntity { get; set; }
        public Entity SecondEntity { get; set; }
        public Entity NestedEntity { get; set; }

        public override string ToString()
        {
            return FirstEntity.Name + GetDescriptor(RelationshipType) + SecondEntity.Name;
        }

        private string GetDescriptor(RelationshipType relationshipType)
        {
            switch (relationshipType)
            {
                case RelationshipType.ManyToMany:
                    return "<>";

                case RelationshipType.OneToMany:
                    return ">";

                case RelationshipType.OneToOne:
                    return "-";

                default:
                    return "";
            }
        }
    }
}