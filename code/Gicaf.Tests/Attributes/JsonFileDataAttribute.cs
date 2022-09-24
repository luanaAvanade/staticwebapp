using Gicaf.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace Gicaf.Tests.Attributes
{
    internal enum DataType
    {
        SingleObject,
        ArrayOfArrays,
        ArrayOfObjects
    }

    internal class JsonFileDataAttribute : DataAttribute
    {
        private const string Dir = "JsonFiles";
        private readonly string _filePath;
        private readonly string _propertyName;
        private readonly DataType _dataType;
        private string _fileFullPath => ../../@"{Dir}\{_filePath}";

        /// <summary>
        /// Load data from a JSON file as the data source for a theory
        /// </summary>
        /// <param name="filePath">The absolute or relative path to the JSON file to load</param>
        /// <param name="propertyName">The name of the property on the JSON file that contains the data for the test</param>
        public JsonFileDataAttribute(string filePath, DataType dataType = DataType.ArrayOfObjects, string propertyName = null)
        {
            _filePath = filePath;
            _dataType = dataType;
            _propertyName = propertyName;
        }

        /// <inheritDoc />
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) { throw new ArgumentNullException(nameof(testMethod)); }

            // Get the absolute path to the JSON file
            var path = Path.IsPathRooted(_fileFullPath)
                ? _fileFullPath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _fileFullPath);

            if (!File.Exists(path))
            {
                throw new ArgumentException(../../"Could not find file at path: {path}");
            }

            // Load the file
            var fileData = File.ReadAllText(path);
            var data = JToken.Parse(fileData);
            var result = new List<object[]>();

            if (!string.IsNullOrWhiteSpace(_propertyName))
            {
                // Only use the specified property as the data
                data = data[_propertyName];
            }

            if(_dataType == DataType.SingleObject)
            {
                Type type = testMethod.GetParameters().FirstOrDefault().ParameterType;
                var obj = data.ToObject(type);
                result.Add(new object[] { obj });
            }
            else if (_dataType == DataType.ArrayOfArrays)
            {
                result = data.ToObject<List<object[]>>();
            }
            else if (_dataType == DataType.ArrayOfObjects)
            {
                var dataItems = data.ToObject<List<object>>();
                foreach (var dataItem in dataItems)
                {
                    Type type = testMethod.GetParameters().FirstOrDefault().ParameterType;
                    var obj = ((JToken)dataItem).ToObject(type);
                    result.Add(new object[] { obj });
                }
                //var objs = data.ToObject<List<dynamic>>();
                //foreach (var obj in objs)
                //{
                //    result.Add(new object[] { obj });
                //}
            }
            

            return result;
        }
    }
}
