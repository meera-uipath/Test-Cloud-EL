# Optional Module — Build the Enrollment Wizard Test Robot Yourself

!!! info "Optional"
    Requires Windows and UiPath Studio Desktop.

**Goal:** build `Run-EnrollmentWizard-Test` yourself with Claude Code and the `uipath-rpa` skill, instead of
using the one your instructor provides.

A coding agent can reason about and write XAML, but it can't browse a live web page visually the way it reads a
PDF. The `uipath-rpa` skill works "discovery-first": you capture the UI targets before the agent generates
anything.

## 1. Set up your working folder

```
mkdir enrollment-wizard-robot
cd enrollment-wizard-robot
```

Open the folder in both Studio Desktop and your coding agent's terminal.

## 2. Capture the target screens first

Use Studio's UI Explorer/Recorder against
[https://accrual-bp-hri.azurewebsites.net/login](https://accrual-bp-hri.azurewebsites.net/login) and walk
through the app by hand. Sign in as `jordan` with the demo password from your instructor. Then step through plan
selection → dependents → beneficiaries → life events → confirm. Save the captured elements to the project's
Object Repository.

## 3. Give the coding agent the build instruction

Replace `<demo-password>` with the password your instructor gave you.

```
Using the uipath-rpa skill, generate a Studio Desktop workflow project named
"Run-EnrollmentWizard-Test" for UI-testing the BenefitConnect enrollment
wizard at https://accrual-bp-hri.azurewebsites.net.

Use the Object Repository elements already captured for: login, plan
selection, dependents, beneficiaries, life events, and confirm.

Inputs:
- in_WizardStep — one of: login | plan_selection | dependents | beneficiaries | life_events | confirm
- in_Persona — one of: jordan | leilani | priya | marcus (all password <demo-password>)
- in_Action — string describing the step-specific action to perform
- in_ExpectedResult — string describing what to verify after the action

Build one dispatcher workflow that switches on in_WizardStep and routes to
one child workflow per step. Each child workflow must:
- Log in as in_Persona if not already authenticated
- Perform in_Action using the existing Object Repository selectors
- Verify in_ExpectedResult using a Get Text / Verify Expression activity
- Wrap the step in Try Catch: a caught exception (e.g. selector not found)
  must set status to "blocked", never "pass" or "fail"

Known business rules to validate when relevant:
- Beneficiary allocation percentages must sum to 100
- Life event submissions must use a valid QLE type

Return output arguments matching:
{
  "id": "string — passed through from the caller",
  "status": "pass | fail | blocked",
  "actual_result": "string",
  "evidence": "string — e.g. screenshot path or captured text"
}

Never default to "pass" on ambiguity — only report pass when the verification
activity actually succeeded.
```

## 4. Watch it scaffold

Approve each step as Claude Code creates the dispatcher and child workflows, wires in your Object Repository
selectors, and adds error handling.

## 5. Debug locally in Studio Desktop

Before you use it in a larger flow, get each of these results at least once:

- A real `pass`
- A real `fail`: for example, enter a beneficiary split that doesn't add up to 100
- A real `blocked`: for example, point it at a stale selector

## 6. Publish to Orchestrator

Publish it as `Run-EnrollmentWizard-Test`.

## 7. Re-wire Module 2

Replace the instructor-provided workflow with your own using `uip tm testcases link-automation`. Then re-run
both sample scenarios end-to-end and check that the results have the same shape.

!!! warning "⚠️ Caveat"
    The CLI and publish subcommands above come from the `uipath-rpa` skill's catalog description and haven't
    been checked in a real run. Confirm them against `uip --help` and the skill's own guidance during a dry run
    before finalizing.
