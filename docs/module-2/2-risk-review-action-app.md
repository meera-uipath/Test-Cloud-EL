# 2. Inspect the "Risk Review" Action App

Open the pre-built Action App and see how it turns structured JSON into a form. As a reviewer, you never see or
edit raw JSON here.

## Read-only fields

- Change title
- Risk level badge
- Plain-language summary
- Bulleted risk factors with detail
- Recommended test priority

## Your input as the reviewer

- A **Decision** dropdown: `Approve as assessed` or `Override risk level`
- If you override, a **Risk Level** dropdown appears, along with a required **Notes** field

## On submit

The app writes this back into the process, and the next agent reads it:

```json
{ "risk_level_final": "...", "override_applied": true, "reviewer_notes": "...", "reviewer": "...", "timestamp": "..." }
```
