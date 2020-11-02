using Serializators.XML;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Vkimow.Tools.Single;

namespace Vkimow.Unity.GOAP
{
    public class GOAPIO : Singleton<GOAPIO>
    {
        public void Load(string path)
        {
            GOAPBlanksManager.Instance.Clear();
            var xDocument = XDocument.Load($"{path}\\GoapBlanks.xml");
            GOAPBlanksManager.Instance.ReadXML(xDocument.Root);

            GOAPActionsManager.Instance.Clear();
            xDocument = XDocument.Load($"{path}\\GoapActions.xml");
            GOAPActionsManager.Instance.ReadXML(xDocument.Root);

            GOAPGoalsManager.Instance.Clear();
            xDocument = XDocument.Load($"{path}\\GoapGoals.xml");
            GOAPGoalsManager.Instance.ReadXML(xDocument.Root);
        }


        public void Save(string path)
        {
            XMLSerializator.Save(GOAPActionsManager.Instance, $"{path}\\GoapActions.xml");
            XMLSerializator.Save(GOAPBlanksManager.Instance, $"{path}\\GoapBlanks.xml");
            XMLSerializator.Save(GOAPGoalsManager.Instance, $"{path}\\GoapGoals.xml");
        }
    }
}
