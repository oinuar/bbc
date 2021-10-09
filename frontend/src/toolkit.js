function returnNull() {
  return null;
}

function dispatcher(dispatch, action, f, ...args) {
  const payload = f(...args);

  // Do not dispatch undefined payloads.
  if (payload !== undefined) dispatch(action(payload));
}

export function useAction(dispatch, action, payloadConstructor) {
  let f;

  if (payloadConstructor === undefined || payloadConstructor === null) f = returnNull;
  else if (typeof payloadConstructor === 'function') f = payloadConstructor;
  else f = () => payloadConstructor;

  return dispatcher.bind(null, dispatch, action, f);
}
