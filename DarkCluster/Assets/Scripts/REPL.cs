using UnityEngine;
using System.Collections;
using Mono.CSharp;
using System.Reflection;
using System.IO;
using System.Text;
using System;

public class REPL : MonoBehaviour {

    StringBuilder _stringBuilder;

	// Use this for initialization
	void Start () {
        var sw = new StringWriter();
        Evaluator.MessageOutput = sw;
        _stringBuilder = sw.GetStringBuilder();
        Evaluator.Init(new string[0]);
        Evaluator.ReferenceAssembly(Assembly.GetExecutingAssembly());

        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                Evaluator.ReferenceAssembly(a);
                Debug.Log("Loaded assembly: " + a.FullName);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Couldn't load assembly: " + a.FullName + " (" + ex.ToString() + ")");
            }
        }

        Evaluator.Run("using UnityEngine;");
        Evaluator.Run("using Mono.CSharp;");
        Evaluator.Run("using System;");
        Evaluator.Run("using System.Text;");
        Evaluator.Run("using System.Collections;");
        Evaluator.Run("using System.Linq;");
        Evaluator.Run("using DarkCluster.Core;");
	}
	
	// Update is called once per frame
	void Update () {
	}

    string _text = "";

    void OnGUI()
    {
        if (Event.current.keyCode == KeyCode.Return && Event.current.type == EventType.KeyDown)
        {
            try
            {
                var result = Evaluator.Evaluate(_text);
                Debug.Log("> " + result.ToString());
            }
            catch
            { }
            
            if (_stringBuilder.Length > 0)
            {
                Debug.Log(_stringBuilder.ToString());
                _stringBuilder.Remove(0, _stringBuilder.Length);
            }
        }

        _text = GUI.TextField(new Rect(20, 400, 300, 30), _text);
    }
}
