using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AutoRescale =false, AccessRights = AccessRights.None)]
    
    public class RHSwingTrendLines : Indicator
    {   
        [Parameter("Swing TimeFrame", DefaultValue = "Hour")]
        public TimeFrame SwingTimeFrame { get; set; }
        
        [Output("BullishLine", LineColor = "#26A69A", PlotType = PlotType.DiscontinuousLine, Thickness = 3)]
        public IndicatorDataSeries BullishLine { get; set; }
        
        [Output("BearishLine", LineColor = "#EF5350", PlotType = PlotType.DiscontinuousLine, Thickness = 3)]
        public IndicatorDataSeries BearishLine { get; set; }

        [Parameter("Draw Candles", DefaultValue = true)]
        public bool DrawCandles { get; set; }

        [Parameter("Draw Trend Lines", DefaultValue = true)]
        public bool DrawTrendLines { get; set; }
        
        private Bars _bars;
        public IndicatorDataSeries SwingDirection { get; set; }
        public IndicatorDataSeries SwingValue { get; set; }

        private double _highestHigh;
        private double _lowestHigh;
        private double _highestLow;
        private double _lowestLow;
        private int _upswing;
        private int _lastSwingIndex;

        protected override void Initialize()
        {
            _bars = MarketData.GetBars(SwingTimeFrame);
            _lastSwingIndex = -1;

            if (_bars.Count > 1)
            {
                _highestHigh = _bars.HighPrices.Last(1);
                _lowestLow = _bars.LowPrices.Last(1);
                _lowestHigh = _bars.HighPrices.Last(1);
                _highestLow = _bars.LowPrices.Last(1);
                _upswing = 0;

                if (_upswing == 1)
                {
                    _highestHigh = _bars.HighPrices.Last(1);
                    _highestLow = _bars.LowPrices.Last(1);
                }
                else
                {
                    _lowestLow = _bars.LowPrices.Last(1);
                    _lowestHigh = _bars.HighPrices.Last(1);
                }
            }
            else 
            {
                _upswing = 0;
                _highestHigh = double.MinValue;
                _lowestLow = double.MaxValue;
                _lowestHigh = double.MinValue;
                _highestLow = double.MaxValue;
            }
        }

        public override void Calculate(int index)
        {
            // Get the corresponding swing timeframe index
            int swingIndex = _bars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);
            
            // Skip if we're still on the same swing bar
            if (swingIndex == _lastSwingIndex)
            {
                // Copy the previous values
                if (index > 0)
                {
                    BullishLine[index] = BullishLine[index - 1];
                    BearishLine[index] = BearishLine[index - 1];
                    SwingDirection[index] = SwingDirection[index - 1];
                    SwingValue[index] = SwingValue[index - 1];
                }
                return;
            }

            _lastSwingIndex = swingIndex;

            if (swingIndex < 0 || swingIndex >= _bars.Count)
                return;

            double highPrice = _bars.HighPrices[swingIndex];
            double lowPrice = _bars.LowPrices[swingIndex];

            UpdateSwingState(highPrice, lowPrice);

            if (DrawCandles)
                DrawCandle(index);

            if (DrawTrendLines)
                DrawLine(index);

            SwingDirection[index] = _upswing;
            SwingValue[index] = _upswing == 1 ? _highestLow : _lowestHigh;
        }

        private void UpdateSwingState(double highPrice, double lowPrice)
        {
            if (_upswing == 1)
            {
                if (highPrice > _highestHigh) _highestHigh = highPrice;
                if (lowPrice > _highestLow) _highestLow = lowPrice;
                if (highPrice < _highestLow)
                {
                    _upswing = 0;
                    _lowestLow = lowPrice;
                    _lowestHigh = highPrice;
                }
            }
            else
            {
                if (lowPrice < _lowestLow) _lowestLow = lowPrice;
                if (highPrice < _lowestHigh) _lowestHigh = highPrice;
                if (lowPrice > _lowestHigh)
                {
                    _upswing = 1;
                    _highestHigh = highPrice;
                    _highestLow = lowPrice;
                }
            }
        }

        private void DrawLine(int index)
        {
            if (_upswing == 1)
                BullishLine[index] = _highestLow;
            else
                BearishLine[index] = _lowestHigh;
        }

        private void DrawCandle(int index)
        {
            if (_upswing == 1)
                Chart.SetBarColor(index, "#26A69A");
            else
                Chart.SetBarColor(index, "#EF5350");
        }
    }
}
