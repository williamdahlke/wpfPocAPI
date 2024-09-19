﻿using AspectInjector.Broker;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using wpfPocAPI.Controllers;
using wpfPocAPI.Models;
using wpfPocAPI.Models.Enums;
using wpfPocAPI.Service;

namespace wpfPocAPI.Interceptors
{
    [Aspect(Scope.Global)]
    [Injection(typeof(MetricInterceptor))]
    public class MetricInterceptor : Attribute
    {
        private const string API_URL = @"http://localhost:3031/api/metrics/insert";
        private Stopwatch _stopwatch = new Stopwatch();
        private Metric _metric;

        [Advice(Kind.Before, Targets = Target.Method)]
        public void OnEntry([Argument(Source.Name)] string name, [Argument(Source.Type)] Type type)
        {
            _stopwatch.Start();
            string fullFileName = type.FullName + "." + name;

            try
            {
                _metric = MetricController.GetMetricByName(fullFileName);
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao capturar métrica");
                return;
            }


            if (_metric.Operation != MetricOperationType.Timer)
            {
                try
                {
                    Task task = Services.Instance.PostJsonAsync(API_URL, _metric);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                _stopwatch.Start();
            }
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void OnExit()
        {
            _stopwatch.Stop();

            if (_metric != null && _metric.Operation == MetricOperationType.Timer && _metric.Type == MetricType.Histogram)
            {
                HistogramMetric histogram = (HistogramMetric)_metric;
                histogram.ElapsedTimeMs = _stopwatch.ElapsedMilliseconds;

                try
                {
                    Task task = Services.Instance.PostJsonAsync(API_URL, _metric);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            _stopwatch.Reset();
        }
    }
}
