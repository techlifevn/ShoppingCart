using System.Data;
using System.Reflection;

namespace DemoApi.Data.Extensions
{
    public static class DataReaderExtensions
    {
        public static List<TModel> GetListData<TModel>(this IDataReader dr)
        {
            List<TModel> result = new();
            string columnName;

            // Get all the properties in <TModel>
            PropertyInfo[] props = typeof(TModel).GetProperties();

            while (dr.Read())
            {
                TModel model = Activator.CreateInstance<TModel>();

                // Loop through columns in data reader
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    // Get field name from data reader
                    columnName = dr.GetName(index);

                    // Get property that matches the field name
                    PropertyInfo property = props.FirstOrDefault(col => col.Name == columnName);

                    if (property != null)
                    {
                        // Get the value from the table
                        var value = dr[columnName];
                        // Assign value to property if not null
                        if (!value.Equals(DBNull.Value))
                        {
                            property.SetValue(model, value);
                        }
                    }
                }
                result.Add(model);
            }
            dr.Close();
            return result;
        }

        public static TModel GetFirstData<TModel>(this IDataReader dr)
        {
            string columnName;

            // Get all the properties in <TModel>
            PropertyInfo[] props = typeof(TModel).GetProperties();

            while (dr.Read())
            {
                TModel model = Activator.CreateInstance<TModel>();

                // Loop through columns in data reader
                for (int index = 0; index < dr.FieldCount; index++)
                {
                    // Get field name from data reader
                    columnName = dr.GetName(index);

                    // Get property that matches the field name
                    PropertyInfo property = props.FirstOrDefault(col => col.Name == columnName);

                    if (property != null)
                    {
                        // Get the value from the table
                        var value = dr[columnName];
                        // Assign value to property if not null
                        if (!value.Equals(DBNull.Value))
                        {
                            property.SetValue(model, value);
                        }
                    }
                }
                dr.Close();
                return model;
            }

            return default;
        }
    }
}
