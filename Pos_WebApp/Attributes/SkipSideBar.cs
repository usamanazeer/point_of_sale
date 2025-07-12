using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Pos_WebApp.Attributes
{
    public class SkipSideBar: Attribute, IFilterMetadata
    {
    }
}
