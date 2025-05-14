# RH-SwingTrendLines

A cTrader indicator that identifies and draws trend lines based on swing highs and lows in the market. This indicator helps traders visualize potential support and resistance levels by connecting significant swing points.

## Features

- Identifies swing highs and lows automatically
- Draws bullish (support) and bearish (resistance) trend lines
- Customizable timeframe for swing detection
- Overlay display on the main chart
- Color-coded trend lines for easy visualization

## Parameters

| Parameter | Description | Default Value |
|-----------|-------------|---------------|
| Swing TimeFrame | The timeframe used to identify swing points | Hour |

## Outputs

The indicator provides two main outputs:

1. **BullishLine** (Green)
   - Represents support levels
   - Drawn during uptrends
   - Color: #26A69A (Green)

2. **BearishLine** (Red)
   - Represents resistance levels
   - Drawn during downtrends
   - Color: #EF5350 (Red)

## How It Works

The indicator analyzes price action to identify swing points:

1. In an uptrend:
   - Tracks the highest high and highest low
   - Draws bullish trend lines at support levels
   - Switches to downtrend when price breaks below the highest low

2. In a downtrend:
   - Tracks the lowest low and lowest high
   - Draws bearish trend lines at resistance levels
   - Switches to uptrend when price breaks above the lowest high

## Usage

1. Add the indicator to your chart
2. Select your preferred Swing TimeFrame
3. The indicator will automatically draw trend lines as swing points are identified

## Notes

- The indicator works best on higher timeframes for swing trading
- Trend lines are drawn as discontinuous lines to clearly show the connection between swing points
- The indicator automatically updates as new price data becomes available

## Requirements

- cTrader platform
- .NET Framework 4.5 or higher