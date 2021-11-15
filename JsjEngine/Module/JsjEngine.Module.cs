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
            _engine.AddHostType("Console", typeof(Console));
            _engine.DocumentSettings.AccessFlags |= DocumentAccessFlags.EnableAllLoading;
            _engine.DocumentSettings.Loader = new JsjDocumentLoader(this);
            _engine.DocumentSettings.LoadCallback = InitiateDocumentInfo;
        }



        private void InitiateDocumentInfo(ref DocumentInfo info)
        {

        }

    }
}
