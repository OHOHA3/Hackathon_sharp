CREATE TABLE hackathons
(
    id       SERIAL PRIMARY KEY,
    harmonic DOUBLE PRECISION NOT NULL
);

CREATE TABLE employees
(
    id           SERIAL PRIMARY KEY,
    name         TEXT    NOT NULL,
    is_team_lead BOOLEAN NOT NULL,
    hackathon_id INTEGER NOT NULL,
    FOREIGN KEY (hackathon_id) REFERENCES hackathons (id) ON DELETE CASCADE
);

CREATE TABLE teams
(
    id           SERIAL PRIMARY KEY,
    junior_id    INTEGER NOT NULL,
    team_lead_id INTEGER NOT NULL,
    FOREIGN KEY (junior_id) REFERENCES employees (id) ON DELETE CASCADE,
    FOREIGN KEY (team_lead_id) REFERENCES employees (id) ON DELETE CASCADE
);

CREATE TABLE wish_lists
(
    id                  SERIAL PRIMARY KEY,
    employee_id         INTEGER NOT NULL,
    desired_employee_id INTEGER NOT NULL,
    grade               INTEGER NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employees (id) ON DELETE CASCADE,
    FOREIGN KEY (desired_employee_id) REFERENCES employees (id) ON DELETE CASCADE
);
