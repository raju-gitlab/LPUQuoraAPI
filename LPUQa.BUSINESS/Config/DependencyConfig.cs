using LPUQa.BUSINESS.Business;
using LPUQa.BUSINESS.IBusiness;
using LPUQa.REPOSITORY.IRepository;
using LPUQa.REPOSITORY.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.BUSINESS.Config
{

    public class DependencyConfig
    {
        public static void SolveDependency(IServiceCollection collection)
        {
            collection.AddSingleton<IAuthBusiness, AuthBusiness>();
            collection.AddSingleton<IAuthRepository, AuthRepository>();
            collection.AddSingleton<IQuoraQuestionsRepository, QuoraQuestionsRepository>();
            collection.AddSingleton<IQuoraQuestionsBusiness, QuoraQuestionsBusiness>();
        }
    }
}
