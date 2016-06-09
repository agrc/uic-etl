using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using uic_etl.models.dtos;

namespace uic_etl.commands
{
    /// <summary>
    ///     Command to find the index for each attribute name
    /// </summary>
    public class FindIndexByFieldNameCommand
    {
        private readonly IFields _fields;
        private readonly IEnumerable<string> _fieldsToMap;
        private readonly Dictionary<string, IndexFieldMap> _propertyValueIndexMap;

        public FindIndexByFieldNameCommand()
        {
            _propertyValueIndexMap = new Dictionary<string, IndexFieldMap>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FindIndexByFieldNameCommand" /> class.
        /// </summary>
        /// <param name="layer"> The layer. </param>
        /// <param name="fieldsToMap">The fields to map to an index number</param>
        public FindIndexByFieldNameCommand(IFeatureClass layer, IEnumerable<string> fieldsToMap) : this()
        {
            _fieldsToMap = fieldsToMap;
            _fields = layer.Fields;
        }

        public FindIndexByFieldNameCommand(IRelationshipClass violationRelation, IEnumerable<string> fieldsToMap) : this()
        {
            _fieldsToMap = fieldsToMap;
            _fields = violationRelation.DestinationClass.Fields;
        }

        /// <summary>
        ///     code to execute when command is run. Iterates over every month and finds the index for the field in teh feature
        ///     class
        /// </summary>
        public Dictionary<string, IndexFieldMap> Execute()
        {
            foreach (var field in _fieldsToMap)
            {
                _propertyValueIndexMap.Add(field,
                                           new IndexFieldMap(GetIndexForField(field, _fields), field));
            }

            return _propertyValueIndexMap;
        }

        /// <summary>
        ///     Gets the index for field.
        /// </summary>
        /// <param name="attributeName"> The attribute name. </param>
        /// <param name="fields"> The fields. </param>
        /// <returns> </returns>
        private static int GetIndexForField(string attributeName, IFields fields)
        {
            var findField = fields.FindField(attributeName.Trim());

            return findField < 0 ? fields.FindFieldByAliasName(attributeName.Trim()) : findField;
        }
    }
}