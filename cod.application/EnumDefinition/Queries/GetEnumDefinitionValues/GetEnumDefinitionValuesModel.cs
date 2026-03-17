namespace cod.application.EnumDefinition.Queries.GetEnumDefinitionValues
{
    public class GetEnumDefinitionValuesModel<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }
    }
}