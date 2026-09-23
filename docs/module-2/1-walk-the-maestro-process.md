# 1. Walk the pre-built Maestro process

Open the process your instructor provided and follow its shape:

```mermaid
flowchart TD
  A[Start: change request received] --> B[Agent Task: change-risk-assessor-agent]
  B --> C{escalate == true?}
  C -->|yes| D[Human Task: Action App - Risk Review]
  C -->|no| E[Agent Task: test-planner-agent]
  D --> E
  E --> F[uip tm: register test cases + test set]
  F --> G[uip tm: link-automation]
  G --> H[uip tm: testsets run]
  H --> I[uip tm: report get]
  I --> J[End: shareable test report]
```

The gateway reads the `escalate` flag produced by the agent you built in Module 1:

- **Low-risk** changes go straight to the test-planner agent.
- **Escalated** changes pause at a human task until a reviewer submits the Risk Review form.
