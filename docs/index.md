# Participant's Handbook

Welcome to the **Agentic Testing Workshop**. In this hands-on session you'll build a chain of
coding-agent-built agents that cover **risk assessment**, **test planning**, and **test execution**, orchestrated
end to end against the live BenefitConnect demo app.

!!! warning "First draft — as of 2026-09-23"
    Sections marked ⚠️ describe mechanics taken from the product documentation that haven't been run end to end
    yet. Verify them before delivering this workshop live.

## What you'll build

| Module | What you'll do |
| --- | --- |
| [Module 1 — Change Risk Assessor](module-1-change-risk-assessor.md) | Learn the **describe → generate → validate → publish → run** loop by building an agent that scores the business risk of a change |
| [Module 2 — Orchestrate & Execute](module-2/index.md) | Chain that agent into a Maestro flow with a human risk review, a test-planner agent, and real UiPath Test Manager execution |
| [Optional Module](optional-module-rpa-robot.md) | Build the Enrollment Wizard test robot yourself with the `uipath-rpa` skill (Windows + Studio Desktop) |

## The test target

Every module works against the **BenefitConnect** demo portal, an HR benefits-enrollment app for a fictional
"Accrual Bank" tenant:

- A login, a dashboard, and a multi-step enrollment wizard (plan selection, dependents, beneficiaries, life events, confirm)
- Real business rules to test: beneficiary allocations must add up to 100%, and life-event submissions must
  use a valid QLE type

Your instructor will share the portal URL and the demo accounts on the [Prerequisites](prerequisites.md) page.

## Sample scenarios

You'll use the same two change requests throughout the workshop:

- **Low risk:** relabel a button in the enrollment wizard. You should see no escalation, and the change goes
  straight through to test planning and execution.
- **High risk:** add a new "Adoption" life event type. You should see it escalate to a human review before test
  planning.

Ready? Start with the [Prerequisites](prerequisites.md).
