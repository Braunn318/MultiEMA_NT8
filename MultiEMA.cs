// MultiEMA – 7 EMA v jednom indikátoru pro NinjaTrader 8
//
// - Každá EMA: perioda, vstup (Open / High / Low / Close / Median / Typical) a zapnutí zvlášť.
// - Barva, tloušťka, styl čáry: skupina „Plots" v okně Indicators (standardní NT8 plot).
// - Výpočet = vestavěný NT8 EMA(vstup, perioda).
//
// 1.0.1 (2026-11-02): oprava kompilace v NT8 (výčet vstupů mimo namespace).
// 1.0.0 (2026-11-02): první verze.
//
// Zdroják: F:\Trading\DEV\MultiEMA Indicator\MultiEMA.cs (github.com/Braunn318/MultiEMA_NT8)
// Instalace: Documents\NinjaTrader 8\bin\Custom\Indicators\Braunn\MultiEMA.cs, kompilace F5 v NinjaScript Editoru.

#region Using declarations
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Data;
#endregion

// Výčet je v globálním prostoru jmen (jako u M2Zones): NT8 generuje kód podle prvního
// prostoru jmen v souboru – jiný před .Braunn by vygeneroval chybné odkazy.
public enum MultiEMASource
{
	Open,
	High,
	Low,
	Close,
	Median,
	Typical
}

// Namespace .Braunn = složka „Braunn" v seznamu indikátorů NT8 (stejně jako M2Zones). Neměnit.
namespace NinjaTrader.NinjaScript.Indicators.Braunn
{
	public class MultiEMA : Indicator
	{
		private const int EmaCount = 7;

		private readonly EMA[] emas = new EMA[EmaCount];

		// V okně Indicators je vidět název; v grafu se vlevo nahoře nevypisuje, pokud není zapnuté ShowChartLabel.
		public override string DisplayName
		{
			get { return State < State.DataLoaded || ShowChartLabel ? Name : string.Empty; }
		}

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description					= "7 EMA v jednom indikátoru (EMA50 Low, EMA50 High, EMA 20/200/300/600/1200 Close). Perioda, vstup, barva a tloušťka každé čáry jdou upravit.";
				Name						= "MultiEMA";
				Calculate					= Calculate.OnPriceChange;
				IsOverlay					= true;
				DisplayInDataBox			= true;
				DrawOnPricePanel			= true;
				PaintPriceMarkers			= true;
				IsSuspendedWhileInactive	= true;
				ScaleJustification			= ScaleJustification.Right;
				ShowChartLabel				= false;

				Show1 = true;	Period1 = 50;	Source1 = MultiEMASource.Low;
				Show2 = true;	Period2 = 50;	Source2 = MultiEMASource.High;
				Show3 = true;	Period3 = 20;	Source3 = MultiEMASource.Close;
				Show4 = true;	Period4 = 200;	Source4 = MultiEMASource.Close;
				Show5 = true;	Period5 = 300;	Source5 = MultiEMASource.Close;
				Show6 = true;	Period6 = 600;	Source6 = MultiEMASource.Close;
				Show7 = true;	Period7 = 1200;	Source7 = MultiEMASource.Close;

				// Výchozí vzhled – vše se mění v okně Indicators → Plots.
				AddPlot(new Stroke(Brushes.DodgerBlue,	1), PlotStyle.Line, "EMA 1 (50 Low)");
				AddPlot(new Stroke(Brushes.DodgerBlue,	1), PlotStyle.Line, "EMA 2 (50 High)");
				AddPlot(new Stroke(Brushes.Gold,		2), PlotStyle.Line, "EMA 3 (20)");
				AddPlot(new Stroke(Brushes.Orange,		2), PlotStyle.Line, "EMA 4 (200)");
				AddPlot(new Stroke(Brushes.OrangeRed,	2), PlotStyle.Line, "EMA 5 (300)");
				AddPlot(new Stroke(Brushes.Magenta,		2), PlotStyle.Line, "EMA 6 (600)");
				AddPlot(new Stroke(Brushes.DarkViolet,	3), PlotStyle.Line, "EMA 7 (1200)");
			}
			else if (State == State.DataLoaded)
			{
				for (int i = 0; i < EmaCount; i++)
					emas[i] = EMA(SeriesFor(SourceAt(i)), Math.Max(1, PeriodAt(i)));
			}
		}

		protected override void OnBarUpdate()
		{
			for (int i = 0; i < EmaCount; i++)
			{
				// Vypnutá EMA se nevykreslí ani neukáže v Data Boxu.
				if (ShowAt(i))
					Values[i][0] = emas[i][0];
			}
		}

		private ISeries<double> SeriesFor(MultiEMASource source)
		{
			switch (source)
			{
				case MultiEMASource.Open:		return Open;
				case MultiEMASource.High:		return High;
				case MultiEMASource.Low:		return Low;
				case MultiEMASource.Median:		return Median;
				case MultiEMASource.Typical:	return Typical;
				default:						return Close;
			}
		}

		private bool ShowAt(int i)
		{
			switch (i)
			{
				case 0: return Show1;
				case 1: return Show2;
				case 2: return Show3;
				case 3: return Show4;
				case 4: return Show5;
				case 5: return Show6;
				default: return Show7;
			}
		}

		private int PeriodAt(int i)
		{
			switch (i)
			{
				case 0: return Period1;
				case 1: return Period2;
				case 2: return Period3;
				case 3: return Period4;
				case 4: return Period5;
				case 5: return Period6;
				default: return Period7;
			}
		}

		private MultiEMASource SourceAt(int i)
		{
			switch (i)
			{
				case 0: return Source1;
				case 1: return Source2;
				case 2: return Source3;
				case 3: return Source4;
				case 4: return Source5;
				case 5: return Source6;
				default: return Source7;
			}
		}

		#region Properties
		// --- EMA 1 ---
		[Display(Name = "Zobrazit", Order = 1, GroupName = "EMA 1")]
		public bool Show1 { get; set; }

		[Range(1, int.MaxValue)]
		[Display(Name = "Perioda", Order = 2, GroupName = "EMA 1")]
		public int Period1 { get; set; }

		[Display(Name = "Vstup", Order = 3, GroupName = "EMA 1")]
		public MultiEMASource Source1 { get; set; }

		// --- EMA 2 ---
		[Display(Name = "Zobrazit", Order = 1, GroupName = "EMA 2")]
		public bool Show2 { get; set; }

		[Range(1, int.MaxValue)]
		[Display(Name = "Perioda", Order = 2, GroupName = "EMA 2")]
		public int Period2 { get; set; }

		[Display(Name = "Vstup", Order = 3, GroupName = "EMA 2")]
		public MultiEMASource Source2 { get; set; }

		// --- EMA 3 ---
		[Display(Name = "Zobrazit", Order = 1, GroupName = "EMA 3")]
		public bool Show3 { get; set; }

		[Range(1, int.MaxValue)]
		[Display(Name = "Perioda", Order = 2, GroupName = "EMA 3")]
		public int Period3 { get; set; }

		[Display(Name = "Vstup", Order = 3, GroupName = "EMA 3")]
		public MultiEMASource Source3 { get; set; }

		// --- EMA 4 ---
		[Display(Name = "Zobrazit", Order = 1, GroupName = "EMA 4")]
		public bool Show4 { get; set; }

		[Range(1, int.MaxValue)]
		[Display(Name = "Perioda", Order = 2, GroupName = "EMA 4")]
		public int Period4 { get; set; }

		[Display(Name = "Vstup", Order = 3, GroupName = "EMA 4")]
		public MultiEMASource Source4 { get; set; }

		// --- EMA 5 ---
		[Display(Name = "Zobrazit", Order = 1, GroupName = "EMA 5")]
		public bool Show5 { get; set; }

		[Range(1, int.MaxValue)]
		[Display(Name = "Perioda", Order = 2, GroupName = "EMA 5")]
		public int Period5 { get; set; }

		[Display(Name = "Vstup", Order = 3, GroupName = "EMA 5")]
		public MultiEMASource Source5 { get; set; }

		// --- EMA 6 ---
		[Display(Name = "Zobrazit", Order = 1, GroupName = "EMA 6")]
		public bool Show6 { get; set; }

		[Range(1, int.MaxValue)]
		[Display(Name = "Perioda", Order = 2, GroupName = "EMA 6")]
		public int Period6 { get; set; }

		[Display(Name = "Vstup", Order = 3, GroupName = "EMA 6")]
		public MultiEMASource Source6 { get; set; }

		// --- EMA 7 ---
		[Display(Name = "Zobrazit", Order = 1, GroupName = "EMA 7")]
		public bool Show7 { get; set; }

		[Range(1, int.MaxValue)]
		[Display(Name = "Perioda", Order = 2, GroupName = "EMA 7")]
		public int Period7 { get; set; }

		[Display(Name = "Vstup", Order = 3, GroupName = "EMA 7")]
		public MultiEMASource Source7 { get; set; }

		// --- Vzhled ---
		[Display(Name = "ShowChartLabel", Description = "Vypisovat název indikátoru vlevo nahoře v grafu.", Order = 1, GroupName = "Vzhled")]
		public bool ShowChartLabel { get; set; }

		// --- Hodnoty pro použití z kódu ---
		[Browsable(false)] [XmlIgnore] public Series<double> Ema1 { get { return Values[0]; } }
		[Browsable(false)] [XmlIgnore] public Series<double> Ema2 { get { return Values[1]; } }
		[Browsable(false)] [XmlIgnore] public Series<double> Ema3 { get { return Values[2]; } }
		[Browsable(false)] [XmlIgnore] public Series<double> Ema4 { get { return Values[3]; } }
		[Browsable(false)] [XmlIgnore] public Series<double> Ema5 { get { return Values[4]; } }
		[Browsable(false)] [XmlIgnore] public Series<double> Ema6 { get { return Values[5]; } }
		[Browsable(false)] [XmlIgnore] public Series<double> Ema7 { get { return Values[6]; } }
		#endregion
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private Braunn.MultiEMA[] cacheMultiEMA;
		public Braunn.MultiEMA MultiEMA()
		{
			return MultiEMA(Input);
		}

		public Braunn.MultiEMA MultiEMA(ISeries<double> input)
		{
			if (cacheMultiEMA != null)
				for (int idx = 0; idx < cacheMultiEMA.Length; idx++)
					if (cacheMultiEMA[idx] != null &&  cacheMultiEMA[idx].EqualsInput(input))
						return cacheMultiEMA[idx];
			return CacheIndicator<Braunn.MultiEMA>(new Braunn.MultiEMA(), input, ref cacheMultiEMA);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.Braunn.MultiEMA MultiEMA()
		{
			return indicator.MultiEMA(Input);
		}

		public Indicators.Braunn.MultiEMA MultiEMA(ISeries<double> input )
		{
			return indicator.MultiEMA(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.Braunn.MultiEMA MultiEMA()
		{
			return indicator.MultiEMA(Input);
		}

		public Indicators.Braunn.MultiEMA MultiEMA(ISeries<double> input )
		{
			return indicator.MultiEMA(input);
		}
	}
}

#endregion
