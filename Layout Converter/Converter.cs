using Layout_Converter.Model;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Layout_Converter
{
    public static class Converter
    {
        public static Layout ParseXML(string xmlCode)
        {
            var xmlDocument = XDocument.Parse(xmlCode);

            var rootLayout = new Layout { DefiniedType = xmlDocument.Root.Name.LocalName };
            rootLayout.ApplyAttributes(xmlDocument.Root.Attributes().ToList());

            ParseXmlAndroidLayout(xmlDocument.Root, rootLayout);

            return rootLayout;
        }

        private static void ParseXmlAndroidLayout(XElement node, Layout parent = null)
        {
            Queue<(XElement, Layout)> procQueue = new Queue<(XElement, Layout)>();

            foreach (var subNode in node.Elements())
            {
                procQueue.Enqueue((subNode, parent));
            }

            while (procQueue.Count > 0)
            {
                // Process node
                var currentNode = procQueue.Dequeue();

                if (!currentNode.Item1.HasElements)
                {
                    // Just create and add view to its parent
                    View v = new View { DefiniedType = currentNode.Item1.Name.LocalName };
                    v.ApplyAttributes(currentNode.Item1.Attributes().ToList());
                    v.Parent = currentNode.Item2;
                    currentNode.Item2.Children.Add(v);
                }
                else
                {
                    // Create and add layout
                    var layout = new Layout { DefiniedType = currentNode.Item1.Name.LocalName };
                    layout.ApplyAttributes(currentNode.Item1.Attributes().ToList());

                    currentNode.Item2.Children.Add(layout);
                    layout.Parent = currentNode.Item2;

                    // Process sub nodes
                    foreach (var subNode in currentNode.Item1.Elements())
                        procQueue.Enqueue((subNode, layout));
                }
            }
        }
    }
}
