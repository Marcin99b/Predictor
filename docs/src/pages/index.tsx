import type { ReactNode } from "react";
import clsx from "clsx";
import Link from "@docusaurus/Link";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Layout from "@theme/Layout";
import Heading from "@theme/Heading";

import styles from "./index.module.css";

function HomepageHeader() {
  const { siteConfig } = useDocusaurusContext();
  return (
    <header className={clsx("hero hero--primary", styles.heroBanner)}>
      <div className="container">
        <Heading as="h1" className="hero__title">
          Finally, a straight answer to "When can I afford it?"
        </Heading>
        <p className="hero__subtitle">
          Budget apps tell you where your money went.
          <br />
          <strong>Predictor tells you when you'll have enough.</strong>
        </p>
        <div className={styles.heroExample}>
          <p className={styles.questionExample}>"I want to buy a $400k house. When will I have the down payment?"</p>
          <p className={styles.answerExample}>
            <strong>November 2027</strong> — if you save $2,000/month starting now.
          </p>
        </div>
        <div className={styles.buttons}>
          <Link className="button button--secondary button--lg" to="/docs/">
            See how it works
          </Link>
          <Link className="button button--outline button--lg" to="/docs/api" style={{ marginLeft: "10px" }}>
            Try the API
          </Link>
        </div>
      </div>
    </header>
  );
}

function PainPointSection() {
  return (
    <section className={styles.painSection}>
      <div className="container">
        <h2 className="text--center margin-bottom--lg">Tired of financial guesswork?</h2>
        <div className="row">
          <div className="col col--4">
            <div className={styles.painCard}>
              <h3>😤 "Someday" isn't a plan</h3>
              <p>
                You know you want to buy a house, but when will you actually have enough saved? Next year? In three
                years? Your spreadsheet is a mess and you're just hoping for the best.
              </p>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.painCard}>
              <h3>📊 Budget apps show the past</h3>
              <p>
                Mint and YNAB are great for tracking where your money went. But they don't tell you when you'll reach
                your goals. You need to know the future, not just the past.
              </p>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.painCard}>
              <h3>🤯 Life is complicated</h3>
              <p>
                Your income changes. Loans get paid off. You get bonuses. Kids need braces. Simple calculators can't
                handle real life scenarios.
              </p>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

function SolutionSection() {
  return (
    <section className={styles.solutionSection}>
      <div className="container">
        <div className="row">
          <div className="col col--6">
            <h2>Input your real situation</h2>
            <div className={styles.inputExample}>
              <div className={styles.inputItem}>
                <strong>Income:</strong> $5,200 salary + $800 freelance work
              </div>
              <div className={styles.inputItem}>
                <strong>Expenses:</strong> $2,100 rent + $400 car payment (ends Dec 2026)
              </div>
              <div className={styles.inputItem}>
                <strong>One-time:</strong> $3,000 tax refund in April
              </div>
              <div className={styles.inputItem}>
                <strong>Starting with:</strong> $8,500 in savings
              </div>
            </div>
          </div>
          <div className="col col--6">
            <h2>Get month-by-month predictions</h2>
            <div className={styles.outputExample}>
              <div className={styles.monthRow}>
                <span>Jan 2025:</span> <span>$11,700</span>
              </div>
              <div className={styles.monthRow}>
                <span>Apr 2025:</span> <span>$23,400</span> <span className={styles.highlight}>← Tax refund month</span>
              </div>
              <div className={styles.monthRow}>
                <span>Dec 2026:</span> <span>$48,900</span> <span className={styles.highlight}>← Car paid off</span>
              </div>
              <div className={styles.monthRow}>
                <span>Jun 2027:</span> <span>$65,300</span>{" "}
                <span className={styles.highlight}>← House down payment ready!</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

function SocialProofSection() {
  return (
    <section className={styles.socialProofSection}>
      <div className="container">
        <h2 className="text--center margin-bottom--lg">Early stage, but already useful</h2>
        <div className="row">
          <div className="col col--4">
            <div className={styles.statCard}>
              <div className={styles.statNumber}>API</div>
              <div className={styles.statLabel}>Ready to use prototype</div>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.statCard}>
              <div className={styles.statNumber}>MIT</div>
              <div className={styles.statLabel}>Open source license</div>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.statCard}>
              <div className={styles.statNumber}>Help</div>
              <div className={styles.statLabel}>Contributors welcome</div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

function UseCaseSection() {
  return (
    <section className={styles.useCaseSection}>
      <div className="container">
        <h2 className="text--center margin-bottom--lg">What you could build with this</h2>
        <div className="row">
          <div className="col col--6">
            <div className={styles.useCaseCard}>
              <h3>Personal Finance App</h3>
              <p className={styles.useCaseQuote}>
                Add financial projections to your budgeting app. Let users see when they'll reach their savings goals
                instead of just tracking past expenses.
              </p>
              <div className={styles.useCaseDetails}>
                <strong>Integration:</strong> REST API calls
                <br />
                <strong>Value:</strong> Goal-oriented planning
                <br />
                <strong>Users love:</strong> Concrete timelines
              </div>
            </div>
          </div>
          <div className="col col--6">
            <div className={styles.useCaseCard}>
              <h3>Financial Advisory Tool</h3>
              <p className={styles.useCaseQuote}>
                Help clients model different scenarios. "What if I take this job?" "When can I retire?" Show the math
                behind the advice.
              </p>
              <div className={styles.useCaseDetails}>
                <strong>Integration:</strong> Scenario comparison
                <br />
                <strong>Value:</strong> Data-driven decisions
                <br />
                <strong>Clients love:</strong> Clear visualizations
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

function TechnicalSection() {
  return (
    <section className={styles.technicalSection}>
      <div className="container">
        <div className="row">
          <div className="col col--6">
            <h2>Prototype, but solid foundation</h2>
            <ul className={styles.featureList}>
              <li>REST API with OpenAPI documentation</li>
              <li>Handles complex scenarios (loans, irregular income)</li>
              <li>Input validation and error handling</li>
              <li>Docker support</li>
              <li>Test coverage for core functionality</li>
              <li>Ready for integration and experimentation</li>
            </ul>
            <div className={styles.callToAction}>
              <Link className="button button--primary" to="/docs/api">
                Try the API
              </Link>
              <Link className="button button--secondary" to="/docs/contributing" style={{ marginLeft: "10px" }}>
                Help improve it
              </Link>
            </div>
          </div>
          <div className="col col--6">
            <div className={styles.codeExample}>
              <div className={styles.codeHeader}>POST /api/v1/predictions</div>
              <pre className={styles.codeBlock}>{`{
  "predictionMonths": 24,
  "initialBudget": 8500,
  "incomes": [
    {
      "name": "Salary",
      "value": 5200,
      "frequency": "Monthly"
    }
  ],
  "expenses": [
    {
      "name": "Rent",
      "value": 2100,
      "frequency": "Monthly"
    }
  ]
}`}</pre>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

function QuickStartSection() {
  return (
    <section className={styles.quickStartSection}>
      <div className="container">
        <h2 className="text--center margin-bottom--lg">Ready to try it?</h2>
        <div className="row">
          <div className="col col--4 text--center">
            <div className={styles.stepCard}>
              <div className={styles.stepNumber}>1</div>
              <h3>Clone & Run</h3>
              <code>
                git clone https://github.com/Marcin99b/Predictor.git
                <br />
                cd Predictor/src
                <br />
                dotnet run --project Predictor.Web
              </code>
            </div>
          </div>
          <div className="col col--4 text--center">
            <div className={styles.stepCard}>
              <div className={styles.stepNumber}>2</div>
              <h3>Open API Docs</h3>
              <code>https://localhost:7176/swagger</code>
              <p style={{ marginTop: "1rem", fontSize: "0.9rem", color: "var(--ifm-color-emphasis-600)" }}>
                Interactive documentation with examples
              </p>
            </div>
          </div>
          <div className="col col--4 text--center">
            <div className={styles.stepCard}>
              <div className={styles.stepNumber}>3</div>
              <h3>Try It Out</h3>
              <code>GET /api/v1/predictions/example</code>
              <p style={{ marginTop: "1rem", fontSize: "0.9rem", color: "var(--ifm-color-emphasis-600)" }}>
                Use the example data to make your first prediction
              </p>
            </div>
          </div>
        </div>
        <div className="text--center" style={{ marginTop: "3rem" }}>
          <Link className="button button--primary button--lg" to="/docs/api">
            View Full Documentation
          </Link>
        </div>
      </div>
    </section>
  );
}

export default function Home(): ReactNode {
  const { siteConfig } = useDocusaurusContext();
  return (
    <Layout
      title="Financial Planning API"
      description="Stop guessing about money. Get real answers with month-by-month financial projections."
    >
      <HomepageHeader />
      <main>
        <PainPointSection />
        <SolutionSection />
        <SocialProofSection />
        <UseCaseSection />
        <TechnicalSection />
        <QuickStartSection />
      </main>
    </Layout>
  );
}
