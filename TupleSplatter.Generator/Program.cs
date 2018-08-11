using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TupleSplatter.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var lastParamIx = 63;
            var ixs = Enumerable.Range(0, lastParamIx).ToList();
            var genericTypes = ixs.Select(ix => $"T{ix}").ToList();
            var itemNames = ixs.Select(ix => $"i{ix}").ToList();

            var result = string.Join(Environment.NewLine, new string[]
            {
                "using System;",
                "",
                "namespace TupleSplatter",
                "{",
                "",                
                "\tpublic static class TupleSplatterExtensions",
                "\t{",
                string.Join(Environment.NewLine, Enumerable.Range(2, lastParamIx-1).Select(cix =>
                {
                    var cGenericTypes = genericTypes.Take(cix).ToList();
                    var cItemNames = itemNames.Take(cix).ToList();
                    var cGenericTypesString = string.Join(", ", cGenericTypes);
                    var cItemNamesString = string.Join(", ", cItemNames);
                    var cTupleItemsString = string.Join(", ", cGenericTypes.Zip(cItemNames, (t, i) => $"{t} {i}"));
                    var cInvokeParamsString = string.Join(", ", cItemNames.Select(i => $"tuple.{i}"));

                    var currentIxLines = new List<string>();

                    if (cix >= 16)
                    {
                        currentIxLines.AddRange(new []
                        {
                            $"\t\tpublic delegate void SplatActionDelegateWith{cix}Params<{cGenericTypesString}>({cTupleItemsString});",
                            $"\t\tpublic delegate TResult SplatFuncDelegateWith{cix}Params<{cGenericTypesString}, TResult>({cTupleItemsString});",
                            $"\t\tpublic static void Splat<{cGenericTypesString}>(this ({cTupleItemsString}) tuple, SplatActionDelegateWith{cix}Params<{cGenericTypesString}> del) => del({cInvokeParamsString});",
                            $"\t\tpublic static TResult Splat<{cGenericTypesString}, TResult>(this ({cTupleItemsString}) tuple, SplatFuncDelegateWith{cix}Params<{cGenericTypesString}, TResult> del) => del({cInvokeParamsString});",
                            $"\t\tpublic static void SplatInvoke<{cGenericTypesString}>(this SplatActionDelegateWith{cix}Params<{cGenericTypesString}> del, ({cTupleItemsString}) tuple) => del({cInvokeParamsString});",
                            $"\t\tpublic static TResult SplatInvoke<{cGenericTypesString}, TResult>(this SplatFuncDelegateWith{cix}Params<{cGenericTypesString}, TResult> del, ({cTupleItemsString}) tuple) => del({cInvokeParamsString});",
                        });
                    }
                    else
                    {
                        currentIxLines.AddRange(new [] {
                            $"\t\tpublic static void Splat<{cGenericTypesString}>(this ({cTupleItemsString}) tuple, Action<{cGenericTypesString}> action) => action({cInvokeParamsString});",
                            $"\t\tpublic static TResult Splat<{cGenericTypesString}, TResult>(this ({cTupleItemsString}) tuple, Func<{cGenericTypesString}, TResult> func) => func({cInvokeParamsString});",
                            $"\t\tpublic static void SplatInvoke<{cGenericTypesString}>(this Action<{cGenericTypesString}> action, ({cTupleItemsString}) tuple) => action({cInvokeParamsString});",
                            $"\t\tpublic static TResult SplatInvoke<{cGenericTypesString}, TResult>(this Func<{cGenericTypesString}, TResult> func, ({cTupleItemsString}) tuple) => func({cInvokeParamsString});",
                        });
                    }

                    return string.Join(Environment.NewLine, currentIxLines);
                })),
                "\t}",
                "}"
            });

            System.IO.File.WriteAllText("TupleSplatterExtensions.cs", result);
        }
    }
}
