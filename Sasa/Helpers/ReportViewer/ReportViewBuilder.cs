﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sasa.Helpers.ReportViewer
{
    public class ReportViewBuilder : IDisposable
    {
        public ReportView ReportContent { get; }

        public Func<IReadOnlyCollection<DataSetValuePair>> RefreshDataSourceCallback { private get; set; }

        public ReportViewBuilder(string source, IReadOnlyCollection<DataSetValuePair> initialDataSource)
        {
            ReportContent = new ReportView(source, initialDataSource);
            ReportContent.Viewer.ReportRefresh += RptViewerOnReportRefresh;
        }

        

        private void RptViewerOnReportRefresh(object sender, CancelEventArgs e)
        {
            if (RefreshDataSourceCallback == null)
                throw new InvalidOperationException("Unable to locate the method that updates this report's data source.\n" +
                    "Use RefreshDataSourceCallback to point to your refresh data source method."
                    );
            ReportContent.UpdateDataSource(RefreshDataSourceCallback());
        }
        
        public void Dispose()
        {
            ReportContent.Viewer.ReportRefresh -= RptViewerOnReportRefresh;
        }
    }
}
