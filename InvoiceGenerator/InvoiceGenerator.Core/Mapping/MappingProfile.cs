using AutoMapper;
using InvoiceGenerator.InvoiceGenerator.Core.Models.Invoice;
using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;

namespace InvoiceGenerator.InvoiceGenerator.Core.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            const decimal TAX_RATE = 20;

            CreateMap<Invoice, InvoiceServiceViewModel>()
              .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate.ToString("yyyy-MM-dd")))
              .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.ToString("yyyy-MM-dd")))
              .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src =>
                  src.InvoiceItems.Sum(i => i.Price * i.Quantity)))
              .ForMember(dest => dest.Tax, opt => opt.MapFrom(src =>
                  src.InvoiceItems.Sum(i => i.Price * i.Quantity * ( TAX_RATE / 100))))
              .ForMember(dest => dest.InvoiceItems, opt => opt.MapFrom(src => src.InvoiceItems));

            CreateMap<InvoiceItem, InvoiceItemViewModel>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src =>
                    src.Quantity * src.Price * (1 + TAX_RATE / 100)));
            // Invoice -> ViewModel
            CreateMap<Invoice, CreateInvoiceViewModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.CompanyAddress))
                .ForMember(dest => dest.CompanyPhone, opt => opt.MapFrom(src => src.CompanyPhone))
                .ForMember(dest => dest.CompanyEmail, opt => opt.MapFrom(src => src.CompanyEmail))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.ClientName))
                .ForMember(dest => dest.ClientAddress, opt => opt.MapFrom(src => src.ClientAddress))
                .ForMember(dest => dest.ClientPhone, opt => opt.MapFrom(src => src.ClientPhone))
                .ForMember(dest => dest.ClientEmail, opt => opt.MapFrom(src => src.ClientEmail))
                .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber))
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.invoiceListItems, opt => opt.MapFrom(src => src.InvoiceItems));

            // ViewModel -> Invoice
            CreateMap<CreateInvoiceViewModel, Invoice>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.CompanyAddress))
                .ForMember(dest => dest.CompanyPhone, opt => opt.MapFrom(src => src.CompanyPhone))
                .ForMember(dest => dest.CompanyEmail, opt => opt.MapFrom(src => src.CompanyEmail))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.ClientName))
                .ForMember(dest => dest.ClientAddress, opt => opt.MapFrom(src => src.ClientAddress))
                .ForMember(dest => dest.ClientPhone, opt => opt.MapFrom(src => src.ClientPhone))
                .ForMember(dest => dest.ClientEmail, opt => opt.MapFrom(src => src.ClientEmail))
                .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber))
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.InvoiceItems, opt => opt.MapFrom(src => src.invoiceListItems));

            // InvoiceItem <-> ViewModel
            CreateMap<InvoiceItem, InvoiceItemViewModel>().ReverseMap();
        }
    }
}

