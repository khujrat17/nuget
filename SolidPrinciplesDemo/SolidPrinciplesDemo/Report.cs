// Report.cs
namespace SolidPrinciplesDemo
{
    public class Report
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public Report(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}

// IReportFormatter.cs
namespace SolidPrinciplesDemo
{
    public interface IReportFormatter
    {
        string Format(Report report);
    }
}

// PlainTextFormatter.cs
namespace SolidPrinciplesDemo
{
    public class PlainTextFormatter : IReportFormatter
    {
        public string Format(Report report)
        {
            return $"{report.Title}\n{report.Content}";
        }
    }
}

// HtmlFormatter.cs
namespace SolidPrinciplesDemo
{
    public class HtmlFormatter : IReportFormatter
    {
        public string Format(Report report)
        {
            return $"<h1>{report.Title}</h1><p>{report.Content}</p>";
        }
    }
}

// IPrinter.cs
namespace SolidPrinciplesDemo
{
    public interface IPrinter
    {
        void Print(Report report);
    }
}

// ConsolePrinter.cs
namespace SolidPrinciplesDemo
{
    public class ConsolePrinter : IPrinter
    {
        public void Print(Report report)
        {
            Console.WriteLine($"Title: {report.Title}");
            Console.WriteLine($"Content: {report.Content}");
        }
    }
}

// ReportPrinter.cs
namespace SolidPrinciplesDemo
{
    public class ReportPrinter
    {
        public void Print(Report report, IReportFormatter formatter)
        {
            Console.WriteLine(formatter.Format(report));
        }
    }
}

// Shape.cs
namespace SolidPrinciplesDemo
{
    public abstract class Shape
    {
        public abstract double Area();
    }

    // Rectangle.cs
    public class Rectangle : Shape
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override double Area()
        {
            return Width * Height;
        }
    }

    // Circle.cs
    public class Circle : Shape
    {
        public double Radius { get; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public override double Area()
        {
            return Math.PI * Radius * Radius;
        }
    }
}

// ReportService.cs
namespace SolidPrinciplesDemo
{
    public class ReportService
    {
        private readonly IPrinter _printer;
        private readonly IReportFormatter _formatter;

        public ReportService(IPrinter printer, IReportFormatter formatter)
        {
            _printer = printer;
            _formatter = formatter;
        }

        public void GenerateReport(Report report)
        {
            var formattedReport = _formatter.Format(report);
            _printer.Print(report);
        }
    }
}