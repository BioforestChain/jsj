using JsjEngine.Module;
using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsjEngine
{
    public partial class JsjRuntime
    {
        private void RegisterModuleApis()
        {
            _v8Engine.DocumentSettings.AccessFlags |= DocumentAccessFlags.EnableAllLoading;
            _v8Engine.DocumentSettings.Loader = new JsjDocumentLoader(this);
            _v8Engine.DocumentSettings.LoadCallback = InitiateDocumentInfo;
            var bn = 0b11_11;
            var un = 22u;
        }



        private void InitiateDocumentInfo(ref DocumentInfo info)
        {

        }

    }
}
