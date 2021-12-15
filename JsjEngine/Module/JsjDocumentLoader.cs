using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JsjEngine.Module
{
    internal class JsjDocumentLoader : DocumentLoader
    {
        private readonly JsjRuntime _engine;
        public JsjDocumentLoader(JsjRuntime engine)
        {
            _engine = engine;
        }
        public override Task<Document> LoadDocumentAsync(DocumentSettings settings, DocumentInfo? sourceInfo, string specifier, DocumentCategory category, DocumentContextCallback contextCallback)
        {
            if(specifier == "jsj-module-format")
            {
                //todo: load module code from database, and add to module cache.
            }

            return Default.LoadDocumentAsync(settings, sourceInfo, specifier, category, contextCallback);
        }


        
    }
}
