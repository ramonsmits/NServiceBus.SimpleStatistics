interface Implementation
{
    void Inc();
    void ErrorInc();
    void SuccessInc();
    void DurationInc(long start, long end);
    void ConcurrencyInc();
    void ConcurrencyDec();
    long Timestamp();
}
