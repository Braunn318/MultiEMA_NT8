# MultiEMA – 7 EMA v jednom indikátoru (NinjaTrader 8)

Jeden indikátor, sedm EMA. U každé jde nastavit perioda, vstup, zapnutí, barva, tloušťka i styl čáry.

## Instalace

1. Stáhni `MultiEMA.cs` z posledního [release](https://github.com/Braunn318/MultiEMA_NT8/releases/latest).
2. Zkopíruj ho do `Documents\NinjaTrader 8\bin\Custom\Indicators\Braunn\`.
3. V NT8 otevři `New → NinjaScript Editor` a stiskni **F5**.
4. V grafu: pravým tlačítkem → **Indicators…** → složka **Braunn** → **MultiEMA** → **Add**.

## Výchozí nastavení

| EMA | Perioda | Vstup |
|---|---|---|
| EMA 1 | 50 | Low |
| EMA 2 | 50 | High |
| EMA 3 | 20 | Close |
| EMA 4 | 200 | Close |
| EMA 5 | 300 | Close |
| EMA 6 | 600 | Close |
| EMA 7 | 1200 | Close |

## Nastavení

- **EMA 1 … EMA 7**: Zobrazit, Perioda, Vstup (Open / High / Low / Close / Median / Typical).
- **Plots**: barva, tloušťka, styl čáry a typ vykreslení každé EMA.
- **Vzhled → ShowChartLabel**: název indikátoru vlevo nahoře (výchozí vypnuto). Hodnoty jsou v Data Boxu.

Výpočet je vestavěný NT8 `EMA(vstup, perioda)`. EMA1200 potřebuje hodně historie – na 1min grafu načti aspoň 3 dny dat.

Názvy čar v Plots (např. „EMA 4 (200)") ukazují výchozí periodu. Po změně periody zůstávají stejné.

## Historie

- **1.0.0** (2026-11-02) – první verze.
