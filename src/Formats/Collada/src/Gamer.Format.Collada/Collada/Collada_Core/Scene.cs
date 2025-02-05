using System;
using System.Xml;
using System.Xml.Serialization;

namespace grendgine_collada
{
    [Serializable, XmlType(AnonymousType = true)]
    public partial class Grendgine_Collada_Evaluate_Scene
    {
        [XmlAttribute("id")] public string ID;
        [XmlAttribute("name")] public string Name;
        [XmlAttribute("sid")] public string sid;
        [XmlAttribute("enable")] public bool Enable;
        [XmlElement(ElementName = "asset")] public Grendgine_Collada_Asset Asset;
        [XmlElement(ElementName = "extra")] public Grendgine_Collada_Extra[] Extra;
        [XmlElement(ElementName = "render")] public Grendgine_Collada_Render[] Render;
    }

    [Serializable, XmlType(AnonymousType = true)]
    public partial class Grendgine_Collada_Instance_Node
    {
        [XmlAttribute("sid")] public string sID;
        [XmlAttribute("name")] public string Name;
        [XmlAttribute("url")] public string URL;
        [XmlAttribute("proxy")] public string Proxy;
        [XmlElement(ElementName = "extra")] public Grendgine_Collada_Extra[] Extra;
    }

    [Serializable, XmlType(AnonymousType = true)]
    public partial class Grendgine_Collada_Instance_Visual_Scene
    {
        [XmlAttribute("sid")] public string sID;
        [XmlAttribute("name")] public string Name;
        [XmlAttribute("url")] public string URL;
        [XmlElement(ElementName = "extra")] public Grendgine_Collada_Extra[] Extra;
    }

    [Serializable, XmlType(AnonymousType = true)]
    public partial class Grendgine_Collada_Library_Nodes
    {
        [XmlAttribute("id")] public string ID;
        [XmlAttribute("name")] public string Name;
        [XmlElement(ElementName = "node")] public Grendgine_Collada_Node[] Node;
        [XmlElement(ElementName = "asset")] public Grendgine_Collada_Asset Asset;
        [XmlElement(ElementName = "extra")] public Grendgine_Collada_Extra[] Extra;
    }

    [Serializable, XmlType(AnonymousType = true)]
    public partial class Grendgine_Collada_Library_Visual_Scenes
    {
        [XmlAttribute("id")] public string ID;
        [XmlAttribute("name")] public string Name;
        [XmlElement(ElementName = "asset")] public Grendgine_Collada_Asset Asset;
        [XmlElement(ElementName = "extra")] public Grendgine_Collada_Extra[] Extra;
        [XmlElement(ElementName = "visual_scene")] public Grendgine_Collada_Visual_Scene[] Visual_Scene;
    }

    [Serializable, XmlType(AnonymousType = true)]
    public partial class Grendgine_Collada_Node
    {
        [XmlAttribute("id")] public string ID;
        [XmlAttribute("sid")] public string sID;
        [XmlAttribute("name")] public string Name;
        [XmlAttribute("type")] public Grendgine_Collada_Node_Type Type; //: [DefaultValue(Grendgine_Collada_Node_Type.NODE)]
        [XmlAttribute("layer")] public string Layer;
        [XmlElement(ElementName = "lookat")] public Grendgine_Collada_Lookat[] Lookat;
        [XmlElement(ElementName = "matrix")] public Grendgine_Collada_Matrix[] Matrix;
        [XmlElement(ElementName = "rotate")] public Grendgine_Collada_Rotate[] Rotate;
        [XmlElement(ElementName = "scale")] public Grendgine_Collada_Scale[] Scale;
        [XmlElement(ElementName = "skew")] public Grendgine_Collada_Skew[] Skew;
        [XmlElement(ElementName = "translate")] public Grendgine_Collada_Translate[] Translate;
        [XmlElement(ElementName = "instance_camera")] public Grendgine_Collada_Instance_Camera[] Instance_Camera;
        [XmlElement(ElementName = "instance_controller")] public Grendgine_Collada_Instance_Controller[] Instance_Controller;
        [XmlElement(ElementName = "instance_geometry")] public Grendgine_Collada_Instance_Geometry[] Instance_Geometry;
        [XmlElement(ElementName = "instance_light")] public Grendgine_Collada_Instance_Light[] Instance_Light;
        [XmlElement(ElementName = "instance_node")] public Grendgine_Collada_Instance_Node[] Instance_Node;
        [XmlElement(ElementName = "asset")] public Grendgine_Collada_Asset Asset;
        [XmlElement(ElementName = "node")] public Grendgine_Collada_Node[] node;
        [XmlElement(ElementName = "extra")] public Grendgine_Collada_Extra[] Extra;
    }

    [Serializable, XmlType(AnonymousType = true)]
    public partial class Grendgine_Collada_Scene
    {
        [XmlElement(ElementName = "instance_visual_scene")] public Grendgine_Collada_Instance_Visual_Scene Visual_Scene;
        [XmlElement(ElementName = "instance_physics_scene")] public Grendgine_Collada_Instance_Physics_Scene[] Physics_Scene;
        [XmlElement(ElementName = "instance_kinematics_scene")] public Grendgine_Collada_Instance_Kinematics_Scene Kinematics_Scene;
        [XmlElement(ElementName = "extra")] public Grendgine_Collada_Extra[] Extra;
    }

    [Serializable, XmlType(AnonymousType = true)]
    public partial class Grendgine_Collada_Visual_Scene
    {
        [XmlAttribute("id")] public string ID;
        [XmlAttribute("name")] public string Name;
        [XmlElement(ElementName = "asset")] public Grendgine_Collada_Asset Asset;
        [XmlElement(ElementName = "extra")] public Grendgine_Collada_Extra[] Extra;
        [XmlElement(ElementName = "evaluate_scene")] public Grendgine_Collada_Evaluate_Scene[] Evaluate_Scene;
        [XmlElement(ElementName = "node")] public Grendgine_Collada_Node[] Node;
    }
}

