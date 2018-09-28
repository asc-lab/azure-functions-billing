using Shared.Invoicing;

namespace Shared.Printing.JsReport
{
    class JsReportRequest
    {
        public Template Template { get; private set; }
        public TemplateOptions Options { get; private set; }
        public Invoice Data { get; private set; }

        public JsReportRequest(Template template, TemplateOptions options, Invoice data)
        {
            Template = template;
            Options = options;
            Data = data;
        }
    }
}
