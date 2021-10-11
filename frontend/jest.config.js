module.exports = {
  verbose: true,
  resetMocks: true,
  testMatch: ['<rootDir>/test/**/*.[jt]s?(x)'],
  testPathIgnorePatterns: ['/__mocks__/'],
  moduleNameMapper: {
    '\\.(jpg|jpeg|png|gif|eot|otf|webp|svg|ttf|woff|woff2|mp4|webm|wav|mp3|m4a|aac|oga)$': '<rootDir>/test/__mocks__/empty.js',
    '\\.(css|less)$': '<rootDir>/test/__mocks__/empty.js',
    '^@/(.*)$': '<rootDir>/src/$1',
  },
};
