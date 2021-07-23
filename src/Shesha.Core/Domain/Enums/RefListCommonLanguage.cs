using Shesha.Domain.Attributes;
using System.ComponentModel;

namespace Shesha.Domain.Enums
{
    [ReferenceList("Shesha.Core", "CommonLanguage")]
    public enum RefListCommonLanguage
    {
        [Description("English")]
        en = 1,

        [Description("IsiZulu")]
        zulu = 2,

        [Description("IsiXhosa")]
        xhosa = 4,

        [Description("Afrikaans")]
        afrikaans = 8,

        [Description("Sepedi")]
        pedi = 16,

        [Description("Sesotho")]
        sotho = 32,

        [Description("Setswana")]
        tswana = 64,

        [Description("Xitsonga")]
        tsonga = 128,

        [Description("IsiSwati")]
        swati = 256,

        [Description("Tshivenda")]
        venda = 512,

        [Description("IsiNdebele")]
        ndebele = 1024,

        [Description("Sign Language")]
        sign = 2048,
    }
}