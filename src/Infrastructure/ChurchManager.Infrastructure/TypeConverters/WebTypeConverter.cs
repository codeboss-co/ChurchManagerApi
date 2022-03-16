using System.ComponentModel;
using ChurchManager.Infrastructure.TypeConverters.Converter;

namespace ChurchManager.Infrastructure.TypeConverters
{
    public class WebTypeConverter : ITypeConverter
    {
        public void Register()
        {
            TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            TypeDescriptor.AddAttributes(typeof(List<double>), new TypeConverterAttribute(typeof(GenericListTypeConverter<double>)));
            TypeDescriptor.AddAttributes(typeof(List<string>), new TypeConverterAttribute(typeof(GenericListTypeConverter<string>)));
            TypeDescriptor.AddAttributes(typeof(bool), new TypeConverterAttribute(typeof(BoolTypeConverter)));

            //dictionaries
            TypeDescriptor.AddAttributes(typeof(Dictionary<int, int>), new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<int, int>)));
            TypeDescriptor.AddAttributes(typeof(Dictionary<string, bool>), new TypeConverterAttribute(typeof(GenericDictionaryTypeConverter<string, bool>)));

           
            //custom attributes


        }

        public int Order => 0;
    }
}
