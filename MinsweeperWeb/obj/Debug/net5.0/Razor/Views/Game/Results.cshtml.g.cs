#pragma checksum "C:\Users\anasanchez\source\repos\AnaOddles\MinesweeperWeb\MinsweeperWeb\Views\Game\Results.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1e5e5f873a8231142bb48fb09903bf11bc6c11f6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Game_Results), @"mvc.1.0.view", @"/Views/Game/Results.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\anasanchez\source\repos\AnaOddles\MinesweeperWeb\MinsweeperWeb\Views\_ViewImports.cshtml"
using MinsweeperWeb;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\anasanchez\source\repos\AnaOddles\MinesweeperWeb\MinsweeperWeb\Views\_ViewImports.cshtml"
using MinsweeperWeb.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1e5e5f873a8231142bb48fb09903bf11bc6c11f6", @"/Views/Game/Results.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b4895bd90db0aa3b850f1cc4b041c606218a1ec5", @"/Views/_ViewImports.cshtml")]
    public class Views_Game_Results : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<bool>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\anasanchez\source\repos\AnaOddles\MinesweeperWeb\MinsweeperWeb\Views\Game\Results.cshtml"
 if (Model)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p>Success</p>\r\n");
#nullable restore
#line 6 "C:\Users\anasanchez\source\repos\AnaOddles\MinesweeperWeb\MinsweeperWeb\Views\Game\Results.cshtml"

}
else
{ 

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p>Failed</p>\r\n");
#nullable restore
#line 11 "C:\Users\anasanchez\source\repos\AnaOddles\MinesweeperWeb\MinsweeperWeb\Views\Game\Results.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<bool> Html { get; private set; }
    }
}
#pragma warning restore 1591
