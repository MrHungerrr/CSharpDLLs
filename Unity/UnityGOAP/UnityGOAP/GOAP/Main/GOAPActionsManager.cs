using Serializators.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vkimow.Tools.Single;

namespace Vkimow.Unity.GOAP
{
    public class GOAPActionsManager: Singleton<GOAPActionsManager>, IXMLSerializable
    {
        public IReadOnlyList<IGOAPReadOnlyAction> Actions => _actions;

        private List<IGOAPAction> _actions;

        public GOAPActionsManager()
        {
            _actions = new List<IGOAPAction>();
        }

        public void Add(IGOAPAction action)
        {
            _actions.Add(action);
        }

        public void Clear()
        {
            _actions = new List<IGOAPAction>();
        }

        internal bool TryGetActionsWithEffect(KeyValuePair<string, GOAPState> needEffect, out List<GOAPAction> resultActions)
        {
            resultActions = new List<GOAPAction>();

            foreach(GOAPAction action in _actions)
            {
                if (action.Effect.Contains(needEffect))
                {
                    resultActions.Add(action);
                }
            }

            if (resultActions.Count != 0)
                return true;
            else
                return false;
        }

        #region XML Serialization
        public XElement ConvertToXML()
        {
            return ConvertToXML("GOAPActions");
        }

        public XElement ConvertToXML(string name)
        {
            var xElement = new XElement(name);

            foreach(var action in _actions)
            {
                xElement.Add(action.ConvertToXML());
            }

            return xElement;
        }

        public void ReadXML(XElement xMainElement)
        {
            foreach(var xElement in xMainElement.Elements())
            {
                var action = new GOAPAction("Temporary Name");
                action.ReadXML(xElement);
                Add(action);
            }
        }
        #endregion
    }
}
