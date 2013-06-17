using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptCs.Contracts;

namespace ScriptCs.DynamicsCrm
{
   public class ScriptPack : IScriptPack
   {
      public void Initialize(IScriptPackSession session)
      {
         var namespaces = new[]
            {
               "Microsoft.Xrm.Sdk.Client",
               "Microsoft.Xrm.Sdk",
               "Microsoft.Xrm.Sdk.Query",
               "Microsoft.Xrm.Sdk.Messages",
            }.ToList();
         namespaces.ForEach(session.ImportNamespace);

         session.AddReference("System.Runtime.Serialization");
      }

      public IScriptPackContext GetContext()
      {
         return new DynamicsCrm();
      }

      public void Terminate()
      {
      }
   }
}
