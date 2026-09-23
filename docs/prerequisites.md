# Prerequisites

Before you start, make sure you have the following installed and set up.

## Tools

- **Node.js (LTS):** check it with `node --version`
- **A coding agent:** this guide uses Claude Code:
  ```
  npm install -g @anthropic-ai/claude-code
  ```
- **The UiPath CLI:**
  ```
  npm install -g @uipath/cli@latest
  ```
  Check it with `uip --version`.

## UiPath coding-agent skills

```
claude plugin marketplace add https://github.com/UiPath/skills
claude plugin install uipath@uipath-marketplace
uip skills install
```

Check that `uipath-agents`, `uipath-maestro-bpmn`, `uipath-human-in-the-loop`, and `uipath-test` appear in the
output.

!!! note
    `uipath-rpa` is only needed for the [Optional Module](optional-module-rpa-robot.md).

## Sign in to the workshop tenant

```
uip login --authority https://cloud.uipath.com --organization <org> -t <tenant>
```

## Product access

- **Studio Web:** to publish, run, and debug agents
- **UiPath Test Manager:** for Module 2
- **Studio Desktop (Windows only):** only needed for the Optional Module

!!! warning "⚠️ Preview capability"
    Test Manager CLI support (`uip tm`) is in Preview. Confirm your tenant has it enabled before Module 2.

## Test target

The BenefitConnect demo portal:
[https://accrual-bp-hri.azurewebsites.net/login](https://accrual-bp-hri.azurewebsites.net/login)

!!! info "Demo accounts"
    Your instructor will give you the demo account usernames and passwords at the start of the session.

## Provided by your instructor

You won't build these yourself:

- A pre-built Maestro/BPMN process shape for Module 2
- A pre-built "Risk Review" Action App
- The `Run-EnrollmentWizard-Test` RPA workflow
- A known-good fallback output for Module 1's agent, in case your own build needs debugging
