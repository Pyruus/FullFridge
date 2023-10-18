using Dapper;
using System.Data;

namespace FullFridge.Model.Handlers
{
    public class IntArrayToListTypeHandler : SqlMapper.TypeHandler<List<int?>>
    {
        public override List<int?> Parse(object value)
        {
            if (value is int[] intArray)
            {
                return intArray.Select(i => (int?)i).ToList();
            }
            return null;
        }

        public override void SetValue(IDbDataParameter parameter, List<int?> value)
        {
            throw new NotImplementedException();
        }
    }
}
