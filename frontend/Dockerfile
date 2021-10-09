FROM node:lts-slim as build

# Install JRE 11 that is required by the SonarQube scanner.
#RUN mkdir -p /usr/share/man/man1 && apt-get update && apt-get install --no-install-recommends -y openjdk-11-jre && apt-get clean

# Update NPM and install SonarQube for static code analysis.
#RUN npm install -g npm sonarqube-scanner

RUN apt-get update -y && apt-get install -y --no-install-recommends python build-essential && apt-get clean -y

WORKDIR /usr/local/app

# Copy application build files.
COPY package.json .
COPY package-lock.json .

# Install dependencies.
RUN npm install

# Audit all packages.
RUN npm audit --audit-level=critical

# Copy application sources.
COPY .eslintrc.js .
COPY .babelrc .
COPY jest.config.js .
COPY tailwind.config.js .
COPY postcss.config.js .
COPY src src
COPY test test
COPY scripts scripts

# Run dev server by default.
ENTRYPOINT [ "npm", "run" ]
CMD [ "dev" ]


FROM build as publish

# Run tests.
RUN npm run test

# Build the application.
RUN npm run build


FROM nginx:1.18.0

# Copy wwwroot.
COPY --from=publish /usr/local/app/dist /usr/share/nginx/html
