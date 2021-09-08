using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace NomaNova.Ojeda.Web.Shared.Validation
{
    public class ServerSideFluentValidation : ComponentBase
    {
        private static readonly char[] Separators = {'.', '['};

        private ValidationMessageStore _messageStore;

        [CascadingParameter] private EditContext CurrentEditContext { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(ServerSideFluentValidation)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. " +
                    $"For example, you can use {nameof(ServerSideFluentValidation)} " +
                    $"inside an {nameof(EditForm)}.");
            }

            _messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += (s, e) =>
                _messageStore.Clear();
            CurrentEditContext.OnFieldChanged += (s, e) =>
                _messageStore.Clear(e.FieldIdentifier);
        }

        public void DisplayErrors(Dictionary<string, List<string>> errors)
        {
            foreach (var (key, value) in errors)
            {
                _messageStore.Add(ToFieldIdentifier(CurrentEditContext, key), value);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void ClearErrors()
        {
            _messageStore.Clear();
            CurrentEditContext.NotifyValidationStateChanged();
        }

        /**
         * Code based on https://github.com/Blazored/FluentValidation by Chris Sainty,
         * which in turn was based on an article by Steve Sanderson
         * (https://blog.stevensanderson.com/2019/09/04/blazor-fluentvalidation/)
         *
         * This method parses property paths like 'SomeProp.MyCollection[123].ChildProp'
         * and returns a FieldIdentifier which is an (instance, propName) pair. For example,
         * it would return the pair (SomeProp.MyCollection[123], "ChildProp"). It traverses
         * as far into the propertyPath as it can go until it finds any null instance.
         */
        private static FieldIdentifier ToFieldIdentifier(EditContext editContext, string propertyPath)
        {
            var obj = editContext.Model;

            while (true)
            {
                var nextTokenEnd = propertyPath.IndexOfAny(Separators);
                if (nextTokenEnd < 0)
                {
                    return new FieldIdentifier(obj, propertyPath);
                }

                var nextToken = propertyPath.Substring(0, nextTokenEnd);
                propertyPath = propertyPath.Substring(nextTokenEnd + 1);

                object newObj;
                if (nextToken.EndsWith("]"))
                {
                    // It's an indexer
                    // This code assumes C# conventions (one indexer named Item with one param)
                    nextToken = nextToken.Substring(0, nextToken.Length - 1);
                    var prop = obj.GetType().GetProperty("Item");

                    if (prop is object)
                    {
                        // we've got an Item property
                        var indexerType = prop.GetIndexParameters()[0].ParameterType;
                        var indexerValue = Convert.ChangeType(nextToken, indexerType);
                        newObj = prop.GetValue(obj, new[] {indexerValue});
                    }
                    else
                    {
                        // If there is no Item property
                        // Try to cast the object to array
                        if (obj is object[] array)
                        {
                            var indexerValue = Convert.ToInt32(nextToken);
                            newObj = array[indexerValue];
                        }
                        else
                        {
                            throw new InvalidOperationException(
                                $"Could not find indexer on object of type {obj.GetType().FullName}.");
                        }
                    }
                }
                else
                {
                    // It's a regular property
                    var prop = obj.GetType().GetProperty(nextToken);
                    if (prop == null)
                    {
                        throw new InvalidOperationException(
                            $"Could not find property named {nextToken} on object of type {obj.GetType().FullName}.");
                    }

                    newObj = prop.GetValue(obj);
                }

                if (newObj == null)
                {
                    // This is as far as we can go
                    return new FieldIdentifier(obj, nextToken);
                }

                obj = newObj;
            }
        }
    }
}