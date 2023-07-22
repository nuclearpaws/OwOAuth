using System.Text;

namespace OwOAuth.Core.Errors;

public sealed class EntityNotFoundError
    : AbstractResponseError
{
    public EntityNotFoundError(
        string entityName,
        params (string propertyName, object propertyValue)[] entityIdentifyingProperties)
        : this(
            entityName,
            entityIdentifyingProperties
                .Select(eip => new EntityIdentifyingProperty(eip.propertyName, eip.propertyValue))
                .ToArray())
    {
    }

    public EntityNotFoundError(
        string entityName,
        params EntityIdentifyingProperty[] entityIdentifyingProperties)
        : base(GetMessage(
            entityName,
            entityIdentifyingProperties))
    {
    }

    private static string GetMessage(
        string entityName,
        IEnumerable<EntityIdentifyingProperty> entityIdentifyingProperties)
    {
        var entityIdentifyingPropertiesMessages = entityIdentifyingProperties
            .Select(eip => eip.GetMessage())
            .ToArray();

        var messageBuilder = new StringBuilder();

        messageBuilder.Append("Cannot find entity ");
        messageBuilder.Append(entityName);
        messageBuilder.Append(" with identifying ");

        var term = entityIdentifyingPropertiesMessages.Length > 1
            ? "properties "
            : "property ";

        messageBuilder.Append(term);

        for (var i = 0; i < entityIdentifyingPropertiesMessages.Length; i++)
        {
            var joiningText = i switch
            {
                _ when i < entityIdentifyingPropertiesMessages.Length - 2 => ", ",
                _ when i == entityIdentifyingPropertiesMessages.Length - 2 => " and ",
                _ => string.Empty,
            };
            messageBuilder.Append(entityIdentifyingPropertiesMessages[i]);
            messageBuilder.Append(joiningText);
        }
        messageBuilder.Append(".");

        return messageBuilder.ToString();
    }

    public sealed class EntityIdentifyingProperty
    {
        public string PropertyName { get; private set; }
        public object PropertyValue { get; private set; }

        public EntityIdentifyingProperty(
            string propertyName,
            object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string GetMessage()
        {
            var message = $"'{PropertyName}' of '{PropertyValue}'";
            return message;
        }
    }
}
