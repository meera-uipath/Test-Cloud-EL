# Agentic Testing Workshop

Participant's guide for the UiPath **Agentic Testing Workshop**. Participants build a chain of coding-agent-built
agents (risk assessment, test planning, and test execution) and orchestrate them end to end against the live
BenefitConnect demo app.

**Published site:** https://meera-uipath.github.io/Test-Cloud-EL/

## Workshop outline

| Module | What participants build |
| --- | --- |
| Prerequisites | Node.js, Claude Code, the UiPath CLI (`uip`) and UiPath coding-agent skills, plus tenant login |
| Module 1: Change Risk Assessor | An agent that scores a change request across five risk dimensions and decides whether it needs human review |
| Module 2: Orchestrate & Execute | A Maestro process linking the risk assessor, a "Risk Review" Action App, a Test Planner agent, and Test Manager execution |
| Optional Module | The Enrollment Wizard test robot (RPA, needs Studio Desktop on Windows) |

**Test target:** BenefitConnect portal at https://accrual-bp-hri.azurewebsites.net/login

## Repository layout

```
.
├── docs/                                  # MkDocs site source (participant guide)
│   ├── index.md
│   ├── prerequisites.md
│   ├── module-1-change-risk-assessor.md
│   ├── module-2/                          # Module 2, split into five steps
│   ├── optional-module-rpa-robot.md
│   ├── you-did-it.md
│   ├── assets/images/                     # Logo, favicon, screenshots
│   ├── stylesheets/extra.css
│   └── javascripts/extra.js
├── hooks/split_cols.py                    # MkDocs hook: [[[ ... |30| ... ]]] two-column shorthand
├── mkdocs.yml                             # Site config and navigation
├── requirements.txt                       # mkdocs + mkdocs-material
├── Agentic-Testing-Workshop-Guide.md      # Single-file draft of the participant guide
├── Agentic-Testing-Workshop-Handover.md   # Design rationale, source material, open items
└── .github/workflows/deploy.yml           # Builds and deploys the site to GitHub Pages
```

## Previewing the site locally

Requires Python 3.

```bash
python -m venv .venv
# Windows: .venv\Scripts\activate    macOS/Linux: source .venv/bin/activate
pip install -r requirements.txt
mkdocs serve
```

Open http://127.0.0.1:8000. The page reloads as you edit files under `docs/`.

Before pushing, check that the site builds the way CI builds it:

```bash
mkdocs build --strict
```

## Authoring notes

- **New pages** go in `docs/` and must be added to `nav` in `mkdocs.yml`.
- **Two-column layouts** use the custom hook syntax. Each delimiter goes on its own line:

  ```
  [[[
  left column content
  |30|
  right column content
  ]]]
  ```

  The number sets the left column's width as a percentage. Supported values are `30`, `50` and `70`.
- **Admonitions, tabs, Mermaid diagrams and code-copy buttons** are enabled. See `markdown_extensions` in
  `mkdocs.yml`.
- Sections marked ⚠️ describe steps that haven't been run end to end yet. Verify them before a live delivery.

## Deployment

Every push to `main` triggers [.github/workflows/deploy.yml](.github/workflows/deploy.yml). The workflow runs
`mkdocs build --strict` and publishes the result to GitHub Pages. You can also start it manually from the
**Actions** tab (`workflow_dispatch`).

## Author

Meera Kannan, UiPath
