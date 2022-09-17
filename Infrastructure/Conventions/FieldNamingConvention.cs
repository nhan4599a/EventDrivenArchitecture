using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using Global.Shared.Extensions;

namespace Infrastructure.Conventions
{
    public class FieldNamingConvention : IMemberMapConvention
    {
        public string Name => GetType().Name;

        public void Apply(BsonMemberMap memberMap)
        {
            memberMap.SetIgnoreIfNull(true);
            var memberName = memberMap.MemberName;
            memberMap.SetElementName(memberName.ToSnakeCase());
        }
    }
}
