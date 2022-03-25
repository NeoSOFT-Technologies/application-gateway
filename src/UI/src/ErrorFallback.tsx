import React, { ErrorInfo } from "react";
interface FallbackProps {
  error?: Error;
  resetErrorBoundary?: () => void;
}

function ErrorFallback(props: FallbackProps) {
  return (
    <div role="alert">
      <p>Oops! Something went wrong:</p>
      <pre>{props.error?.message}</pre>
      <button onClick={props.resetErrorBoundary}>Try again</button>
    </div>
  );
}

export const ErrorHandler = (error: Error, errorInfo: ErrorInfo) => {
  // TODO - Inject Logger Service to Log Error
  console.log("Logging:", error, errorInfo);
};

export default ErrorFallback;
