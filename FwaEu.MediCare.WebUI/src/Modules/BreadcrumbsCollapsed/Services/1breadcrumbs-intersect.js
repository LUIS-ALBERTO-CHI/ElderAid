export const onIntersect = (
    elementToWatch,
    callback = (target) => { },
    outCallback = (target) => { },
    once = false,
    options = { threshold: 1.0 }
) => {
    let isInit = false;

    const observerCallback = ([entry]) => {
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
    };

    const observerOptions = options || { threshold: 1.0 };

    if (elementToWatch instanceof Element) {
        const observer = new IntersectionObserver(observerCallback, observerOptions);
        observer.observe(elementToWatch);
        return observer;
    } else {
        console.error('Invalid elementToWatch parameter');
        return null;
    }
};
