using ApplicationTestingILT.ObjectRepository;
using System;
using System.Collections.Generic;
using System.Data;
using UiPath.CodedWorkflows;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;
using UiPath.Testing;
using UiPath.Testing.Activities;
using UiPath.Testing.Enums;
using UiPath.UIAutomationNext.API.Contracts;
using UiPath.UIAutomationNext.API.Models;
using UiPath.UIAutomationNext.Enums;

namespace ApplicationTestingILT.CodedTestCases
{
    public class TC_Coded_Login : CodedWorkflow
    {
        [TestCase]
        public void Execute()
        {
            // Arrange
            Log("Test run started for TC_Coded_Login.");

            // Act
            // For accessing UI Elements from Object Repository, you can use the Descriptors class e.g:
            // var screen = uiAutomation.Open(Descriptors.MyApp.FirstScreen);
            // screen.Click(Descriptors.MyApp.FirstScreen.SettingsButton);
            var screen = uiAutomation.Open(Descriptors.UiBank.Login);
            screen.TypeInto(Descriptors.UiBank.Login.Username, "demodemo");
            screen.TypeInto(Descriptors.UiBank.Login.Password, "1qaz@WSX");
            screen.Click(Descriptors.UiBank.Login.Sign_In);

            
            // Assert
            // To start using activities, use IntelliSense (CTRL + Space) to discover the available services, e.g. testing.VerifyExpression(...).
            var accountsScreen = uiAutomation.Attach(Descriptors.UiBank.Accounts);
            var welcomeMessage =  accountsScreen.GetText(Descriptors.UiBank.Accounts.Welcome_message);
            testing.VerifyExpression(welcomeMessage.Contains("Welcome")); //verify
            Log("Welcome message is: " + welcomeMessage);
            
        }
    }
}