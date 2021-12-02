using JsjEngine.Module;
using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine
{
    public partial class JsjEngine
    {
        private void RegisterModuleApis()
        {
            _v8Engine.AddHostType("Console", typeof(Console));
            _v8Engine.DocumentSettings.AccessFlags |= DocumentAccessFlags.EnableAllLoading;
            _v8Engine.DocumentSettings.Loader = new JsjDocumentLoader(this);
            _v8Engine.DocumentSettings.LoadCallback = InitiateDocumentInfo;
        }



        private void InitiateDocumentInfo(ref DocumentInfo info)
        {

        }

    }
}
