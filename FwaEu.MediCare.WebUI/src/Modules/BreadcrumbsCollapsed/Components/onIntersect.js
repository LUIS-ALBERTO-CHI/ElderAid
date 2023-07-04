export const onIntersect = (
    elementToWatch,
    callback = (target) => { },
    outCallback = (target) => { },
    once = false,
    options = { threshold: 1.0 }
) => {
    let isInit = false;

    const observer = new IntersectionObserver(([entry]) => {
        if (!isInit) {
            isInit = true;

            if (entry && entry.isIntersecting) {
                return;
            }
        }

        if (entry && entry.isIntersecting) {
            callback(entry.target);

            if (once) {
                observer.unobserve(entry.target);
            }
        } else {
            outCallback(entry.target);
        }
    }, options);

    observer.observe(elementToWatch);

    return observer;
};
