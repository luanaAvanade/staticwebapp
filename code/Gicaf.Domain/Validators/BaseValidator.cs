
using Gicaf.Domain.Resources;
using FluentValidation;
using FluentValidation.Resources;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Text;

namespace Gicaf.Domain.Validators
{

    public class StringResource : IStringSource
    {
        string _name;
        public StringResource(string name)
        {
            _name = name;
        }

        public string ResourceName => nameof(Resource);

        public Type ResourceType => typeof(Resource);

        public string GetString(IValidationContext context)
        {
            return Resource.ResourceManager.GetName(_name);
        }
    }

    public interface IContextValidator<T>: IValidator<T>
    {
        //string[] Contexts { get; set; }
        ValidationResult ValidateFromContext(string context, T istance);
    }

    public abstract class BaseValidator<T>: AbstractValidator<T>, IContextValidator<T>
    {
        public string[] Contexts { get; set; }

        public static class Consts
        {
            const string PropertyName = nameof(PropertyName);
        }
        protected const string PropertyName = nameof(PropertyName);
        protected const string PropertyValue = nameof(PropertyValue);
        protected const string ExpectedPrecision = nameof(ExpectedPrecision);
        protected const string ExpectedScale = nameof(ExpectedScale);
        protected const string Digits = nameof(Digits);
        protected const string ActualScale = nameof(ActualScale);
        protected const string From = nameof(From);
        protected const string To = nameof(To);

        protected static string DEFAULT_RANGE_ERROR_MSG = "MSG_Valicacao_Intervalo";//../../"{PropertyName} deve estar entre {From} e {To}";

        //public BaseValidator(bool addDefaultRules)
        public BaseValidator(string[] contexts)
        {
            Contexts = contexts;
            //if (addDefaultRules)
            //{
            //    DefaultRules();
            //}
            ////var descriptor = CreateDescriptor();
            ////descriptor.GetName

            //return;
            foreach (var prop in typeof(T).GetProperties())
            {
                var teste = CreateDescriptor();
                var rules = teste.GetRulesForMember(prop.Name);
                foreach (var rule in rules)
                {
                    //rule.RuleSets.
                }
            }
        }

        //public abstract void DefaultRules();

        public IRuleBuilderInitial<T, TProperty> RuleDisplayFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var rule = RuleFor(expression);
            var resource = Resource.ResourceManager.GetName(expression.GetPropertyName());
            rule.Configure(x => x.DisplayName = new StringResource(resource));
            return rule;
        }

        public ValidationResult ValidateFromContext(string context, T instance)
        {
            if (context == null || Contexts == null || Contexts.Contains(context))
            {
                return Validate(instance);
            }
            return new ValidationResult();
        }
    }

    public static class ext
    {
        public static IRuleBuilderOptions<T, TProperty> WithMessageFromResource<T,TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string errorMessage)
        {
            //Resource.
            return rule.WithMessage(Resource.ResourceManager.GetString(errorMessage,CultureInfo.CurrentCulture));
        }
    }
}
