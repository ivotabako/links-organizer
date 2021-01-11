using LinksOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LinksOrganizer.Utils
{
    public interface IBackButtonHandler
    {
        Task<bool> HandleBackButton(ViewModelBase viewModel);
    }
}
