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
            <p style={{ marginBottom: "1rem", color: "var(--ifm-color-emphasis-700)" }}>
              Handle complex financial scenarios that simple calculators can't manage:
            </p>
            <div className={styles.inputExample}>
              <div className={styles.inputItem}>
                <strong>Regular income:</strong> $5,200 salary + $800 freelance work
              </div>
              <div className={styles.inputItem}>
                <strong>Changing expenses:</strong> $2,100 rent + $400 car payment (ends Dec 2026)
              </div>
              <div className={styles.inputItem}>
                <strong>Irregular events:</strong> $3,000 tax refund in April + $500 quarterly insurance
              </div>
              <div className={styles.inputItem}>
                <strong>Current savings:</strong> $8,500 to start with
              </div>
            </div>
          </div>
          <div className="col col--6">
            <h2>Get detailed month-by-month predictions</h2>
            <p style={{ marginBottom: "1rem", color: "var(--ifm-color-emphasis-700)" }}>
              See exactly when you'll hit your goals and how your finances evolve:
            </p>
            <div className={styles.outputExample}>
              <div className={styles.monthRow}>
                <div className={styles.monthInfo}>
                  <span className={styles.monthLabel}>Jan 2025:</span>
                  <span className={styles.monthAmount}>$11,700</span>
                </div>
                <span className={styles.monthDetail}>+$3,200 saved</span>
              </div>
              <div className={styles.monthRow}>
                <div className={styles.monthInfo}>
                  <span className={styles.monthLabel}>Apr 2025:</span>
                  <span className={styles.monthAmount}>$23,400</span>
                </div>
                <span className={styles.highlight}>← Tax refund boost</span>
              </div>
              <div className={styles.monthRow}>
                <div className={styles.monthInfo}>
                  <span className={styles.monthLabel}>Dec 2026:</span>
                  <span className={styles.monthAmount}>$48,900</span>
                </div>
                <span className={styles.highlight}>← Car payment ends</span>
              </div>
              <div className={styles.monthRow}>
                <div className={styles.monthInfo}>
                  <span className={styles.monthLabel}>Jun 2027:</span>
                  <span className={styles.monthAmount}>$65,300</span>
                </div>
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
        <h2 className="text--center margin-bottom--lg">Open source financial planning API</h2>
        <div className="row">
          <div className="col col--4">
            <div className={styles.statCard}>
              <div className={styles.statIcon}>🚀</div>
              <div className={styles.statLabel}>Production ready prototype</div>
              <p className={styles.statDescription}>Core features working, tested, and documented</p>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.statCard}>
              <div className={styles.statIcon}>⚖️</div>
              <div className={styles.statLabel}>MIT License</div>
              <p className={styles.statDescription}>Free to use, modify, and distribute</p>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.statCard}>
              <div className={styles.statIcon}>🤝</div>
              <div className={styles.statLabel}>Looking for contributors</div>
              <p className={styles.statDescription}>Help build the future of financial planning tools</p>
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
        <h2 className="text--center margin-bottom--lg">Who uses financial predictions?</h2>
        <div className="row">
          <div className="col col--4">
            <div className={styles.useCaseCard}>
              <h3>🏠 Home Buyers</h3>
              <p className={styles.useCaseQuote}>"When will I have enough for a down payment?"</p>
              <p>
                Instead of guessing, get exact dates. Factor in your salary, current savings, monthly expenses, and even
                irregular bonuses to see your real timeline.
              </p>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.useCaseCard}>
              <h3>💼 Career Changers</h3>
              <p className={styles.useCaseQuote}>"Can I afford to take that lower-paying job I love?"</p>
              <p>
                Model your new salary against current expenses. See how much longer it takes to reach goals, or if you
                need to adjust your lifestyle first.
              </p>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.useCaseCard}>
              <h3>🎓 Students with Loans</h3>
              <p className={styles.useCaseQuote}>"When will I finally be debt-free?"</p>
              <p>
                Track multiple loan payments with different end dates. Watch your cash flow improve as each debt
                disappears and plan for life after loans.
              </p>
            </div>
          </div>
        </div>
        <div className="row" style={{ marginTop: "2rem" }}>
          <div className="col col--4">
            <div className={styles.useCaseCard}>
              <h3>🚀 Entrepreneurs</h3>
              <p className={styles.useCaseQuote}>"How long can my savings last while building this business?"</p>
              <p>
                Model irregular income and varying expenses. See if you can survive the first year, or if you need more
                runway before quitting your day job.
              </p>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.useCaseCard}>
              <h3>👨‍👩‍👧‍👦 Growing Families</h3>
              <p className={styles.useCaseQuote}>"How will childcare costs affect our house-buying plans?"</p>
              <p>
                Add new expenses like daycare and see the real impact on your timeline. Plan for when expenses end (kids
                start school) and cash flow improves.
              </p>
            </div>
          </div>
          <div className="col col--4">
            <div className={styles.useCaseCard}>
              <h3>💰 Emergency Planners</h3>
              <p className={styles.useCaseQuote}>"When will I have 6 months of expenses saved?"</p>
              <p>
                Set a concrete savings goal and see exactly when you'll reach it. Account for irregular income and
                unexpected expenses along the way.
              </p>
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
        <div className="row">
          <div className="col col--6">
            <h2>Try it now</h2>
            <p style={{ fontSize: "1.1rem", marginBottom: "2rem", color: "var(--ifm-color-emphasis-700)" }}>
              Get Predictor running locally in under 2 minutes. Requires .NET 8 or Docker.
            </p>

            <div className={styles.quickStartTabs}>
              <div className={styles.tabContent}>
                <h3>With .NET 8</h3>
                <div className={styles.commandLine}>
                  <span className={styles.prompt}>$</span>
                  <span className={styles.command}>git clone https://github.com/Marcin99b/Predictor.git</span>
                </div>
                <div className={styles.commandLine}>
                  <span className={styles.prompt}>$</span>
                  <span className={styles.command}>cd Predictor/src && dotnet run --project Predictor.Web</span>
                </div>

                <h3 style={{ marginTop: "2rem" }}>With Docker</h3>
                <div className={styles.commandLine}>
                  <span className={styles.prompt}>$</span>
                  <span className={styles.command}>docker run -p 8080:8080 predictor:latest</span>
                </div>
              </div>
            </div>

            <div className={styles.nextSteps}>
              <p>
                <strong>Then visit:</strong> <code>localhost:7176/swagger</code> for interactive API docs
              </p>
            </div>
          </div>

          <div className="col col--6">
            <div className={styles.featureHighlights}>
              <h3>What you get</h3>
              <ul className={styles.checkList}>
                <li>REST API with comprehensive documentation</li>
                <li>Interactive Swagger interface</li>
                <li>Example data to test with immediately</li>
                <li>Support for complex financial scenarios</li>
                <li>JSON responses with detailed breakdowns</li>
              </ul>

              <div className={styles.ctaButtons}>
                <Link className="button button--primary button--lg" to="/docs/">
                  Read the Docs
                </Link>
                <Link
                  className="button button--outline button--lg"
                  href="https://github.com/Marcin99b/Predictor"
                  style={{ marginTop: "0.5rem" }}
                >
                  View on GitHub
                </Link>
              </div>
            </div>
          </div>
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
