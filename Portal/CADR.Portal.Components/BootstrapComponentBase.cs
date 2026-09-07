using Microsoft.AspNetCore.Components;
using CADR.Portal.Components.Enums;
using CADR.Portal.Components.Enums.Flex;
using CADR.Portal.Components.Enums.Font;
using CADR.Portal.Components.Enums.Text;
using CADR.Portal.Components.Infrastructures;

namespace CADR.Portal.Components
{
    /// <summary>
    /// Базовый компонент сетки Bootstrap
    /// </summary>
    public class BootstrapComponentBase : ComponentBase
    {
        private const string ArgumentCountExceptionMessage = "Свойство {0} содержит недопустимое количество значений";

        private const string ArgumentSpaceContainsExceptionMessage =
            "Свойство {0} содержит недопустимое значение. Только '* 0 1 2 3 4 5 a' являются допустимыми";

        private const string ValidSpacePropertyValues = "*012345a";

        private const string ArgumentBorderContainsExceptionMessage =
            "Свойство {0} содержит недопустимое значение. Только '* 0 1 2 3 4 5' являются допустимыми";

        private const string ValidBorderPropertyValues = "*012345";

        /// <summary>
        /// Параметры компонента
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? CapturedAttributes { get; set; }

        /// <summary>
        /// Получает или задает значение пространства между границей элемента и его дочерним элементом
        /// </summary>
        [Parameter] public string Padding { get; set; } = string.Empty;

        /// <summary>
        /// Получает или задает значение внешнего поля элемента
        /// </summary>
        [Parameter] public string Margin { get; set; } = string.Empty;

        /// <summary>
        /// Получает или задает отступы между столбцами, используемые для
        /// адаптивного размещения и выравнивания содержимого
        /// </summary>
        [Parameter] public string Gutter { get; set; } = string.Empty;

        /// <summary>
        /// Получает или задает отступы дочерним элементам
        /// </summary>
        [Parameter] public string Gap { get; set; } = string.Empty;

        /// <summary>
        /// Получает или задает значение внешней границы
        /// </summary>
        [Parameter] public string Border { get; set; } = string.Empty;

        /// <summary>
        /// Цвет границы
        /// </summary>
        [Parameter] public ThemeColors? BorderColor { get; set; }

        /// <summary>
        /// Скругление рамки
        /// </summary>
        [Parameter] public BorderRadius? BorderRadius { get; set; }

        /// <inheritdoc cref="RoundedSizes"/>
        [Parameter]
        public RoundedSizes RoundedSize { get; set; } = RoundedSizes.Rounded0;

        /// <summary>
        /// Цвет текста
        /// </summary>
        [Parameter] public ThemeColors? Foreground { get; set; }

        /// <summary>
        /// Цвет фона
        /// </summary>
        [Parameter] public ThemeColors? Background { get; set; }

        /// <inheritdoc cref="TextAlignments"/>
        [Parameter] public TextAlignments? TextAlignment { get; set; }

        /// <inheritdoc cref="TextWraps"/>
        [Parameter] public TextWraps? TextWrapping { get; set; }

        /// <inheritdoc cref="TextDecorations"/>
        [Parameter] public TextDecorations? TextDecoration { get; set; }

        /// <inheritdoc cref="TextTransforms"/>
        [Parameter] public TextTransforms? TextTransform { get; set; }

        /// <inheritdoc cref="FontStyles"/>
        [Parameter] public FontStyles? FontStyle { get; set; }

        /// <inheritdoc cref="FontDisplays"/>
        [Parameter] public FontDisplays? FontHeader { get; set; }

        /// <inheritdoc cref="FontSizes"/>
        [Parameter] public FontSizes? FontSize { get; set; }

        /// <inheritdoc cref="LineHeights"/>
        [Parameter] public LineHeights? LineHeight { get; set; }

        /// <summary>Характеристики столбца по умолчанию</summary>
        [Parameter] public Columns? Column { get; set; }

        /// <summary>Характеристики столбца для <see langword="small"/></summary>
        [Parameter] public Columns? ColumnSmall { get; set; }

        /// <summary>Характеристики столбца для <see langword="medium "/></summary>
        [Parameter] public Columns? ColumnMedium { get; set; }

        /// <summary>Характеристики столбца для <see langword="large"/></summary>
        [Parameter] public Columns? ColumnLarge { get; set; }

        /// <summary>Характеристики столбца для <see langword="extra large"/></summary>
        [Parameter] public Columns? ColumnExtraLarge { get; set; }

        /// <summary>Характеристики столбца для <see langword="extra extra large"/></summary>
        [Parameter] public Columns? ColumnExtraExtraLarge { get; set; }

        /// <summary>Адаптивный макет</summary>
        [Parameter] public Flexes? Flex { get; set; }

        /// <summary>Направление адаптивного макета</summary>
        [Parameter] public FlexDirections? FlexDirection { get; set; }

        /// <inheritdoc cref="FlexJustify"/>
        [Parameter] public FlexJustify? FlexContentJustify { get; set; }

        /// <inheritdoc cref="FlexAlign"/>
        [Parameter] public FlexAlign? FlexItemsAlign { get; set; }

        /// <inheritdoc cref="FlexSelf"/>
        [Parameter] public FlexSelf? FlexContentSelf { get; set; }

        /// <inheritdoc cref="FlexFills"/>
        [Parameter] public FlexFills? FlexContentFill { get; set; }

        /// <inheritdoc cref="FlexGrow"/>
        [Parameter] public FlexGrow? FlexContentGrow { get; set; }

        /// <inheritdoc cref="FlexWraps"/>
        [Parameter] public FlexWraps? FlexWrap { get; set; }

        /// <inheritdoc cref="FlexAlignContent"/>
        [Parameter] public FlexAlignContent? FlexContentAlign { get; set; }

        /// <inheritdoc cref="Displays"/>
        [Parameter] public Displays? Display { get; set; }

        /// <summary>Отображение компонентов для <see langword="small"/></summary>
        [Parameter] public Displays? DisplaySmall { get; set; }

        /// <summary>Отображение компонентов для <see langword="medium "/></summary>
        [Parameter] public Displays? DisplayMedium { get; set; }

        /// <summary>Отображение компонентов для <see langword="large"/></summary>
        [Parameter] public Displays? DisplayLarge { get; set; }

        /// <summary>Отображение компонентов для <see langword="extra large"/></summary>
        [Parameter] public Displays? DisplayExtraLarge { get; set; }

        /// <summary>Отображение компонентов для <see langword="extra extra large"/></summary>
        [Parameter] public Displays? DisplayExtraExtraLarge { get; set; }

        /// <inheritdoc cref="Floats"/>
        [Parameter] public Floats? Float { get; set; }

        /// <inheritdoc cref="Shadows"/>
        [Parameter] public Shadows? Shadow { get; set; }

        /// <inheritdoc cref="Overflows"/>
        [Parameter] public Overflows? Overflow { get; set; }

        /// <summary>
        /// Базовый css класс компонента
        /// </summary>
        protected string BaseCssClass { get; set; } = string.Empty;

        /// <summary>
        /// Css классы наследника
        /// </summary>
        protected IList<string> InheritCss { get; private set; } = new List<string>();

        /// <inheritdoc />
        protected override Task OnParametersSetAsync()
        {
            var cssBuilder = new List<string>();
            if (!string.IsNullOrWhiteSpace(Padding))
            {
                cssBuilder.AddRange(GetSpacingValues(nameof(Padding), Padding.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(Margin))
            {
                cssBuilder.AddRange(GetSpacingValues(nameof(Margin), Margin.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(Gutter))
            {
                cssBuilder.AddRange(GetSpacingValues(nameof(Gutter), Gutter.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(Gap))
            {
                cssBuilder.AddRange(GetSpacingValues(nameof(Gap), Gap.Trim(), true));
            }

            if (!string.IsNullOrWhiteSpace(Border))
            {
                cssBuilder.AddRange(GetBorderValues());
            }

            if (!string.IsNullOrWhiteSpace(Border) || Background.HasValue)
            {
                cssBuilder.Add(RoundedSize.ToCssClass());
            }

            TextWrapping.TryAddCssValue(cssBuilder);
            Foreground.TryAddCssValue(cssBuilder, "text-");
            Background.TryAddCssValue(cssBuilder, "bg-");
            TextAlignment?.TryAddCssValue(cssBuilder);
            TextDecoration.TryAddCssValue(cssBuilder);
            TextTransform.TryAddCssValue(cssBuilder);
            FontStyle.TryAddCssValue(cssBuilder);
            FontHeader.TryAddCssValue(cssBuilder);
            FontSize.TryAddCssValue(cssBuilder);
            LineHeight.TryAddCssValue(cssBuilder);
            Column.TryAddCssValue(cssBuilder, "col-");
            ColumnSmall.TryAddCssValue(cssBuilder, "col-sm-");
            ColumnMedium.TryAddCssValue(cssBuilder, "col-md-");
            ColumnLarge.TryAddCssValue(cssBuilder, "col-lg-");
            ColumnExtraLarge.TryAddCssValue(cssBuilder, "col-xl-");
            ColumnExtraExtraLarge.TryAddCssValue(cssBuilder, "col-xxl-");
            Flex.TryAddCssValue(cssBuilder);
            FlexDirection.TryAddCssValue(cssBuilder);
            FlexContentJustify.TryAddCssValue(cssBuilder);
            FlexItemsAlign.TryAddCssValue(cssBuilder);
            FlexContentSelf.TryAddCssValue(cssBuilder);
            FlexContentFill.TryAddCssValue(cssBuilder);
            FlexContentGrow.TryAddCssValue(cssBuilder);
            FlexWrap?.TryAddCssValue(cssBuilder);
            FlexContentAlign.TryAddCssValue(cssBuilder);
            Display.TryAddCssValue(cssBuilder, "d-");
            DisplaySmall.TryAddCssValue(cssBuilder, "d-sm-");
            DisplayMedium.TryAddCssValue(cssBuilder, "d-md-");
            DisplayLarge.TryAddCssValue(cssBuilder, "d-lg-");
            DisplayExtraLarge.TryAddCssValue(cssBuilder, "d-xl-");
            DisplayExtraExtraLarge.TryAddCssValue(cssBuilder, "d-xxl-");
            Float?.TryAddCssValue(cssBuilder);
            Shadow.TryAddCssValue(cssBuilder);
            Overflow.TryAddCssValue(cssBuilder);

            if (InheritCss.Any(x => !string.IsNullOrEmpty(x)))
            {
                cssBuilder.AddRange(InheritCss.Where(x => !string.IsNullOrEmpty(x)));
            }

            if (CapturedAttributes is not null &&
                CapturedAttributes.TryGetValue("class", out var nativeClasses))
            {
                BaseCssClass = string.Join(" ", cssBuilder, nativeClasses).Trim();
                CapturedAttributes.Remove("class");
            }
            else
            {
                BaseCssClass = string.Join(" ", cssBuilder).Trim();
            }
            return base.OnParametersSetAsync();
        }

        private IEnumerable<string> GetSpacingValues(string property, string valueTemplate, bool fullProp = false)
        {
            var breakpointParts = valueTemplate.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var breakpointPart in breakpointParts)
            {
                var breakpoint = string.Empty;
                var breakpointTemplate = breakpointPart.Trim();
                if (breakpointTemplate.IndexOf(':') >= 1)
                {
                    var breakpoints = breakpointTemplate.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    breakpoint = breakpoints[0].Trim().ToLower();
                    breakpointTemplate = breakpoints[1].Trim().ToLower();
                }

                yield return CreateCssSpacingClass(property, breakpoint, breakpointTemplate, fullProp);
            }
        }

        private static string CreateCssSpacingClass(string property,
            string breakpoint,
            string breakpointTemplate,
            bool fullProp)
        {
            string GetSpacingValue(string spacingChar)
                => spacingChar == "a"
                    ? "auto"
                    : spacingChar;
            if (string.IsNullOrWhiteSpace(breakpointTemplate))
            {
                throw new ArgumentNullException(property);
            }

            var values = breakpointTemplate.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 3)
            {
                throw new ArgumentException(string.Format(ArgumentCountExceptionMessage, property), property);
            }

            if (!values.All(x => x.Length == 1 && ValidSpacePropertyValues.Contains(x)))
            {
                throw new ArgumentException(string.Format(ArgumentSpaceContainsExceptionMessage, property), property);
            }

            if (values.Length == 1)
            {
                return values[0] == "*"
                    ? string.Empty
                    : string.Concat((fullProp ? property.ToLower() : property.ToLower()[0]),
                        string.IsNullOrWhiteSpace(breakpoint) ? string.Empty : $"-{breakpoint}",
                        $"-{GetSpacingValue(values[0])}");
            }

            if (values.Length == 2)
            {
                var cssItems = new List<string>(values.Length);
                var sides = "x";
                foreach (var value in values)
                {
                    if (value != "*")
                    {
                        cssItems.Add(string.Concat((fullProp ? property.ToLower() : property.ToLower()[0]),
                            sides,
                            string.IsNullOrWhiteSpace(breakpoint) ? string.Empty : $"-{breakpoint}",
                            $"-{GetSpacingValue(value)}"));
                    }

                    sides = "y";
                }

                return cssItems.Any()
                    ? string.Join(" ", cssItems)
                    : string.Empty;
            }

            var cssResults = new List<string>(values.Length);
            var cssTemplate = "steb"; // [s]tart [t]op [e]nd [b]ottom
            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] != "*")
                {
                    cssResults.Add(string.Concat((fullProp ? property.ToLower() : property.ToLower()[0]),
                        cssTemplate[i],
                        string.IsNullOrWhiteSpace(breakpoint) ? string.Empty : $"-{breakpoint}",
                        $"-{GetSpacingValue(values[i])}"));
                }
            }

            return cssResults.Any()
                ? string.Join(" ", cssResults)
                : string.Empty;
        }

        private IEnumerable<string> GetBorderValues()
        {
            var property = nameof(Border);
            var borderValues = Border.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (borderValues.Length == 3)
            {
                throw new ArgumentException(string.Format(ArgumentCountExceptionMessage, property), property);
            }

            if (!borderValues.All(x => x.Length == 1 && ValidBorderPropertyValues.Contains(x)))
            {
                throw new ArgumentException(string.Format(ArgumentBorderContainsExceptionMessage, property), property);
            }

            var maxBorderWidth = borderValues.Where(x => !x.Equals("*"))
                .Select(int.Parse)
                .DefaultIfEmpty(0)
                .Max();

            if (borderValues.All(x => x.Equals("*")) || maxBorderWidth == 0)
            {
                yield break;
            }

            if (borderValues.Length == 1)
            {
                yield return "border";
            }
            else
            {
                var borderNames = new[] { "start", "top", "end", "bottom" };
                var fourBorderValues = borderValues.Length == 2
                    ? new[] { borderValues[0], borderValues[1], borderValues[0], borderValues[1], }
                    : borderValues;
                for (var i = 0; i < fourBorderValues.Length; i++)
                {
                    if (!"*0".Contains(fourBorderValues[i]))
                    {
                        yield return $"border-{borderNames[i]}";
                    }
                }
            }

            if (BorderColor.HasValue)
            {
                yield return $"border-{BorderColor.ToCssClass()}";
            }

            if (maxBorderWidth > 1)
            {
                yield return $"border-{maxBorderWidth}";
            }

            if (BorderRadius.HasValue)
            {
                yield return BorderRadius.ToCssClass();
            }
        }
    }
}
